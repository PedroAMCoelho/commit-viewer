using System.Threading.Tasks;

namespace CommitViewer.Services.GitHubService
{
    public interface IGitHubService
    {
        Task<string> GetGitHubCommits(string owner, string repository, int page, int page_results);
    }
}
