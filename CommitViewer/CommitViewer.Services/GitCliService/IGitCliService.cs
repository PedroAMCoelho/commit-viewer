using System.Threading.Tasks;

namespace CommitViewer.Services.GitCliService
{
    public interface IGitCliService
    {
        Task<string> GetLocalCommits(string owner, string repository, int page, int page_results);
    }
}
