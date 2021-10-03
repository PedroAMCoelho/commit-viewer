using AutoMapper;
using CommitViewer.Business.Mappings;
using CommitViewer.Business.Models;
using CommitViewer.Services.GitCliService;
using CommitViewer.Services.GitHubService;
using CommitViewer.Services.GitHubService.Options;
using CommitViewer.Shared.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommitViewer.Business.CommitViewer
{
    public class CommitViewerBusiness : ICommitViewerBusiness
    {
        private readonly IGitHubService gitHubService;
        private readonly IGitCliService gitCliService;
        protected virtual IMapper Mapper { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommitViewerBusiness"/> class.
        /// </summary>
        public CommitViewerBusiness(
            IGitHubService gitHubService,
            IGitCliService gitCliService,
            IMapper mapper)
        {
            this.gitHubService = gitHubService;
            this.gitCliService = gitCliService;
            this.Mapper = mapper;
        }

        public async Task<IEnumerable<CommitModel>> GetCommits(string owner, string repository, int page, int page_results)
        {
            //ToDo: Map of git cli response to CommitModel.
            //ToDo: GitHub first and fallback to gitCli service
            var localCommitsStr = await gitCliService.GetLocalCommits(owner, repository, page, page_results);

            return await GetGitHubApiCommits(owner, repository, page, page_results);
        }

        private async Task<IEnumerable<CommitModel>> GetGitHubApiCommits(string owner, string repository, int page, int page_results)
        {
            var commitsResponse = await gitHubService.GetGitHubCommits(owner, repository, page, page_results);
            return CommitModelServiceResponseMapping.JsonResponseToCommitModelList(JsonConvert.DeserializeObject<JArray>(commitsResponse));
        }
    }
}
