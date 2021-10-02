using CommitViewer.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommitViewer.Business.CommitViewer
{
    public interface ICommitViewerBusiness
    {
        Task<IEnumerable<CommitModel>> GetCommits(string owner, string repository, int page, int page_results);
    }
}
