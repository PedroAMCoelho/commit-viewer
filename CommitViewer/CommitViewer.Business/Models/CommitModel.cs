using System;

namespace CommitViewer.Business.Models
{
    public class CommitModel
    {
        public string Sha { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }

        public string AuthorName { get; set; }
    }
}
