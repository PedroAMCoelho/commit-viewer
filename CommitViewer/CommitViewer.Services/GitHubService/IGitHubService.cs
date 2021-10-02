using CommitViewer.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommitViewer.Services.GitHubService
{
    public interface IGitHubService
    {
        Task<IEnumerable<CommitModel>> GetGitHubCommits(string owner, string repository, int page, int page_results);
    }
}
