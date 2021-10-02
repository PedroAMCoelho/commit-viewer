using AutoMapper;
using CommitViewer.Business.Mappings;
using CommitViewer.Business.Models;
using CommitViewer.Services.GitHubService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommitViewer.Business.CommitViewer
{
    public class CommitViewerBusiness : ICommitViewerBusiness
    {
        private readonly IGitHubService gitHubService;
        protected virtual IMapper Mapper { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommitViewerBusiness"/> class.
        /// </summary>
        public CommitViewerBusiness(
            IGitHubService gitHubService, 
            IMapper mapper)
        {
            this.gitHubService = gitHubService;
            this.Mapper = mapper;
        }

        public async Task<IEnumerable<CommitModel>> GetCommits(string owner, string repository, int page, int page_results)
        {
            //ToDo: GitCLI and persistence logic

            return await GetGitHubApiCommits(owner, repository, page, page_results);
        }

        private async Task<IEnumerable<CommitModel>> GetGitHubApiCommits(string owner, string repository, int page, int page_results)
        {
            var commitsResponse = await gitHubService.GetGitHubCommits(owner, repository, page, page_results);
            return CommitModelServiceResponseMapping.JsonResponseToCommitModelList(JsonConvert.DeserializeObject<JArray>(commitsResponse));
        }
    }
}
