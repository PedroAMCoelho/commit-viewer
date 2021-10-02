using CommitViewer.Business.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommitViewer.Business.Mappings
{
    public static class CommitModelServiceResponseMapping
    {
        public static IEnumerable<CommitModel> JsonResponseToCommitModelList(JArray json) => json?.Select(t => t.ToCommitModel()).ToArray();

        private static CommitModel ToCommitModel(this JToken jobject)
        {
            var author = jobject["commit"]["author"];

            return new CommitModel()
            {
                Sha = (string)jobject["sha"],
                AuthorName = (string)author["name"],
                Message = (string)jobject["commit"]["message"],
                Date = (DateTime)author["date"]
            };
        }
    }
}
