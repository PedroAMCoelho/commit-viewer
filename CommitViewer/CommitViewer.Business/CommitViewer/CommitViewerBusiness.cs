using CommitViewer.Business.Mappings;
using CommitViewer.Business.Models;
using CommitViewer.Services.GitCliService;
using CommitViewer.Services.GitCliService.Constants;
using CommitViewer.Services.GitHubService;
using CommitViewer.Shared.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommitViewer.Business.CommitViewer
{
    public class CommitViewerBusiness : ICommitViewerBusiness
    {
        private readonly IGitHubService gitHubService;
        private readonly IGitCliService gitCliService;
        protected ILogger<CommitViewerBusiness> logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommitViewerBusiness"/> class.
        /// </summary>
        public CommitViewerBusiness(
            IGitHubService gitHubService,
            IGitCliService gitCliService,
            ILogger<CommitViewerBusiness> logger)
        {
            this.gitHubService = gitHubService;
            this.gitCliService = gitCliService;
            this.logger = logger;
        }

        public async Task<IEnumerable<CommitModel>> GetCommits(string owner, string repository, int page, int page_results)
        {
            var fallback = Policy<IEnumerable<CommitModel>>
                .Handle<Exception>()
                .FallbackAsync(async (ct) =>
                {
                    logger.LogError($"The call to {nameof(GetGitCliCommits)} failed. Preparing to call {nameof(GetGitCliCommits)} as a fallback");
                    return await GetGitCliCommits(owner, repository, page, page_results);
                });

            var commits = await fallback
                .ExecuteAsync(async () =>
                {
                    return await GetGitHubApiCommits(owner, repository, page, page_results);
                });

            return commits;
        }

        private async Task<IEnumerable<CommitModel>> GetGitHubApiCommits(string owner, string repository, int page, int page_results)
        {
            var commitsResponse = await gitHubService.GetGitHubCommits(owner, repository, page, page_results);
            return CommitModelServiceResponseMapping.JsonResponseToCommitModelList(JsonConvert.DeserializeObject<JArray>(commitsResponse));
        }

        private async Task<IEnumerable<CommitModel>> GetGitCliCommits(string owner, string repository, int page, int page_results)
        {
            var localCommitsStr = Task.Run(async () =>
            {
                return await gitCliService.GetLocalCommits(owner, repository, page, page_results);
            });

            bool hasTimedOut = !localCommitsStr.Wait(TimeSpan.FromMilliseconds(GitCliServiceConstants.GitCliServiceTimeout));

            if (hasTimedOut) 
                throw new TimeoutException($"{nameof(GetGitCliCommits)} has taken longer than the maximum time allowed.");
            else 
                return await JsonExtensions.StringJsonToMap<List<CommitModel>>(await localCommitsStr);                
        }
    }
}
