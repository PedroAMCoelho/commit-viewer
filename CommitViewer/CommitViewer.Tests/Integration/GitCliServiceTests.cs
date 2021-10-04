using CommitViewer.Business.CommitViewer;
using CommitViewer.Services.GitCliService;
using CommitViewer.Services.GitHubService;
using CommitViewer.Shared.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CommitViewer.Tests.Integration
{
    public class GitCliServiceTests
    {
        private readonly Mock<IGitCliService> gitCliServiceMock;

        public GitCliServiceTests()
        {
            gitCliServiceMock = new Mock<IGitCliService>();
        }

        [Fact]
        public async Task Should_Successfully_Obtain_Git_Log()
        {
            var gitCliResponseExample = StreamReaderExtensions.ReadFileJsonString("gitCliServiceResponseExample.txt");

            gitCliServiceMock.Setup(s => s.GetLocalCommits("octocat", "hello-world", 1, 1)).ReturnsAsync(gitCliResponseExample);

            var result = await gitCliServiceMock.Object.GetLocalCommits("octocat", "hello-world", 1, 1);

            gitCliServiceMock.Verify(x => x.GetLocalCommits(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());

            Assert.IsAssignableFrom<string>(result);
        }
    }
}
