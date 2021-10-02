using CommitViewer.Services.GitHubService;
using CommitViewer.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommitViewer.Business.CommitViewer
{
    public class CommitViewerBusiness : ICommitViewerBusiness
    {
        private readonly IGitHubService gitHubService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommitViewerBusiness"/> class.
        /// </summary>
        public CommitViewerBusiness(
            IGitHubService gitHubService)
        {
            this.gitHubService = gitHubService;
        }

        public async Task<IEnumerable<CommitModel>> GetCommits(string owner, string repository, int page, int page_results)
        {
            //ToDo: GitCLI and persistence logic

            return await gitHubService.GetGitHubCommits(owner, repository, page, page_results);
        }
    }
}
