using CommitViewer.Business.Mappings;
using CommitViewer.Business.Models;
using CommitViewer.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CommitViewer.Tests.Business
{
    public class CommitModelMappingsTests
    {
        [Fact]
        public void GitHubApi_Response_Should_Be_Successfully_Mapped_Into_CommitModel()
        {
            //Arrange
            var gitHubResponseExample = StreamReaderExtensions.ReadFileJsonString("githubResponseExample.txt");

            var mappedRes = CommitModelServiceResponseMapping.JsonResponseToCommitModelList(JsonConvert.DeserializeObject<JArray>(gitHubResponseExample));

            //Assert
            Assert.Equal("7fd1a60b01f91b314f59955a4e4d4e80d8edf11d", mappedRes.First().Sha);
            Assert.Equal("The Octocat", mappedRes.First().AuthorName);
            Assert.Contains("Merge pull request #6 from Spaceghost", mappedRes.First().Message);
            Assert.Equal(DateTime.Parse("2012-03-06 15:06:50 -0800"), mappedRes.First().Date);
        }

        [Fact]
        public async Task GitCliService_Response_Should_Be_Successfully_Mapped_Into_CommitModel()
        {
            //Arrange
            var gitCliResponseExample = StreamReaderExtensions.ReadFileJsonString("gitCliServiceResponseExample.txt");

            var mappedRes = await JsonExtensions.StringJsonToMap<List<CommitModel>>(gitCliResponseExample);

            //Assert
            Assert.Equal("7fd1a60b01f91b314f59955a4e4d4e80d8edf11d", mappedRes.First().Sha);
            Assert.Equal("The Octocat", mappedRes.First().AuthorName);
            Assert.Equal("Merge-pull-request-6-from-Spaceghost-patch-1", mappedRes.First().Message);
            Assert.Equal(DateTime.Parse("2012-03-06 15:06:50 -0800"), mappedRes.First().Date);
        }
    }
}
