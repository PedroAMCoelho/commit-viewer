using CommitViewer.Services.Constants;
using CommitViewer.Services.GitCliService.Exceptions;
using CommitViewer.Services.GitHubService.Options;
using CommitViewer.Shared.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
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
                
                var localRepositoryPath = GetExistingRepositoryPath(repository);
                string newRepositoryPath = null;

                var hasLocalRepository = !string.IsNullOrWhiteSpace(localRepositoryPath);

                if (hasLocalRepository)
                {
                    logger.LogDebug($"Local git repository found for {repository} in the path {localRepositoryPath}");
                }
                else
                {
                    logger.LogDebug($"No git repository found for {repository}. A clone action will be performed on {url}");
                    newRepositoryPath = await CreateRepositoryPath(url);
                    Directory.CreateDirectory(newRepositoryPath);
                }

                // formating command with input parameters
                string command = string.Format(hasLocalRepository ? GitConstants.GitPullCommand : GitConstants.GitCloneCommand,
                    hasLocalRepository ? localRepositoryPath : newRepositoryPath,
                    url,
                    page * page_results + page_results,
                    GitConstants.JsonFormatGitLog,
                    page * page_results,
                    page_results);

                string gitLog = await this.GetGitLog(command);

                return gitLog;
            }
            catch (Exception exp)
            {
                throw new GitCliServiceException($"{nameof(GitCliService)} process {nameof(GetLocalCommits)} returned errors. Check inner exception.", exp);
            }
        }

        private string GetExistingRepositoryPath(string repository)
        {
            string cmd = string.Format(GitConstants.FindLocalRepo, repository);

            logger.LogDebug($"Creating a temporary powershell file");
            string tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".ps1";
            File.WriteAllText(tempFilePath, cmd);

            logger.LogDebug($"Executing the temporary powershell file with command: {cmd}");
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = $"/C Set-ExecutionPolicy Unrestricted -Scope Process; {tempFilePath}";
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            logger.LogDebug($"Preparing process to read the powershell script output");
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();            

            string errors = process.StandardError.ReadToEnd();

            string repositoryPath = string.IsNullOrWhiteSpace(output) ? null : output.Substring(0, output.IndexOf("\\.git"));

            // ToDo: if there are no errors, return output (error handling)
            // ToDo: Get only necessary part of the output
            return repositoryPath;
        }

        private async Task<string> CreateRepositoryPath(string url)
        {
            Uri uri = new Uri(url);
            string path = Regex.Replace(uri.PathAndQuery, ".git", "", RegexOptions.IgnoreCase).TrimStart('/');
            return Path.GetTempPath() + path.Replace('/', '\\');
        }

        private async Task<string> GetGitLog(string cmd)
        {
            logger.LogDebug($"Creating a temporary .bat file");
            string tempFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".bat";
            File.WriteAllText(tempFilePath, cmd);

            logger.LogDebug($"Executing the temporary .bat file with command: {cmd}");
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd", $"/C {tempFilePath}");
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;            

            logger.LogDebug($"Preparing process to read the .bat script output");
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            string output = null;
            try
            {
                output = await process.StandardOutput.ReadToEndAsync();
                string errors = process.StandardError.ReadToEnd();
            }
            catch(Exception ex)
            {
                logger.LogDebug($"error", ex);
            }

            var firstNodeStartIndex = output.IndexOf("\"sha\":") - 2;
            string gitLog = output.Substring(firstNodeStartIndex);
            return $"[{gitLog}]";
        }
    }
}
