using CommitViewer.Services.GitCliService.Constants;
using CommitViewer.Services.GitCliService.Exceptions;
using CommitViewer.Services.GitHubService.Options;
using CommitViewer.Shared.Extensions;
using CommitViewer.Shared.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommitViewer.Services.GitCliService
{
    public class GitCliService : IGitCliService
    {
        protected ILogger<GitCliService> logger { get; }

        public GitCliService(ILogger<GitCliService> logger)
        {
            this.logger = logger;
        }

        public async Task<string> GetLocalCommits(string owner, string repository, int page, int page_results)
        {
            try
            {
                string url = $"{OptionsConfig<GitHubOptions>.Options.BaseUrl}{owner}/{repository}.git";
                
                var localRepositoryPath = await GetExistingRepositoryPath(repository);
                string newRepositoryPath = null;

                var hasLocalRepository = !string.IsNullOrWhiteSpace(localRepositoryPath);

                if (hasLocalRepository)
                {
                    logger.LogDebug($"Local git repository found for {repository} in the path {localRepositoryPath}");
                }
                else
                {
                    logger.LogDebug($"No git repository found for {repository}. A clone action will be performed on {url}");
                    newRepositoryPath = CreateRepositoryPath(url);
                    Directory.CreateDirectory(newRepositoryPath);
                }
                
                string command = string.Format(hasLocalRepository ? GitCliServiceConstants.GitPullCommand : GitCliServiceConstants.GitCloneCommand,
                    hasLocalRepository ? localRepositoryPath : newRepositoryPath,
                    url,
                    page * page_results + page_results,
                    GitCliServiceConstants.JsonFormatGitLog,
                    page * page_results,
                    page_results);

                logger.LogDebug($"Preparing to get {url} Git Log");
                return await GetGitLog(command);
            }
            catch (Exception exp)
            {
                throw new GitCliServiceException($"{nameof(GitCliService)} process {nameof(GetLocalCommits)} returned errors. Check inner exception.", exp);
            }
        }

        private async Task<string> GetExistingRepositoryPath(string repository)
        {
            string cmd = string.Format(GitCliServiceConstants.FindLocalRepo, repository);

            logger.LogDebug($"Creating a temporary powershell file");
            string tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".ps1";
            File.WriteAllText(tempFilePath, cmd);

            logger.LogDebug($"Executing the temporary powershell file with command: {cmd}");
            Process process = new Process();
            process.StartInfo = ProcessExtensions.CreateProcessStartInfo(@"powershell.exe", $"/C Set-ExecutionPolicy Unrestricted -Scope Process; {tempFilePath}");
            process.Start();

            logger.LogDebug($"Preparing to read the powershell script output");
            string output = await process.StandardOutput.ReadToEndAsync();
            string errors = await process.StandardError.ReadToEndAsync();

            ValidateScriptExecution(cmd, nameof(GitCliServiceConstants.FindLocalRepo), errors);

            return string.IsNullOrWhiteSpace(output) ? null : output.Substring(0, output.IndexOf("\\.git"));
        }

        private async Task<string> GetGitLog(string cmd)
        {
            logger.LogDebug($"Creating a temporary .bat file");
            string tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".bat";
            File.WriteAllText(tempFilePath, cmd);

            logger.LogDebug($"Executing the temporary .bat file with command: {cmd}");            
            Process process = new Process();
            process.StartInfo = ProcessExtensions.CreateProcessStartInfo("cmd", $"/C {tempFilePath}");
            process.Start();

            logger.LogDebug($"Preparing to read the .bat script output");
            string output = await process.StandardOutput.ReadToEndAsync();
            string errors = await process.StandardError.ReadToEndAsync();

            string gitLog = null;

            ValidateScriptExecution(cmd, nameof(GetGitLog), errors);

            if (output.Length > 0)
            {
                logger.LogInformation($"The git command {cmd} succeeded. Response message: {output}");
                var firstNodeStartIndex = output.IndexOf("\"sha\":") - 2;
                gitLog = output.Substring(firstNodeStartIndex);
            }

            return $"[{gitLog}]";
        }

        private string CreateRepositoryPath(string url)
        {
            Uri uri = new Uri(url);
            string path = Regex.Replace(uri.PathAndQuery, ".git", "", RegexOptions.IgnoreCase).TrimStart('/');
            return Path.GetTempPath() + path.Replace('/', '\\');
        }

        private void ValidateScriptExecution(string cmd, string commandDescription, string errors)
        {
            if (errors.Length > 0)
            {
                logger.LogError($"The {commandDescription} command {cmd} failed. Error message: {errors}");
                throw new GitCliServiceException($"The {commandDescription} command {cmd} failed. Error message: {errors}", new GitCliServiceException(), HttpStatusCode.BadRequest);
            }
        }
    }
}
