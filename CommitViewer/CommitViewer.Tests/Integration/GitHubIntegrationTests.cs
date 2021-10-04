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
    public class GitHubIntegrationTests
    {
        private readonly Mock<IGitHubService> gitHubServiceMock;
        private readonly Mock<IGitCliService> gitCliServiceMock;
        private readonly Mock<ILogger<CommitViewerBusiness>> LoggerMock;
        private readonly CommitViewerBusiness commitViewerBusiness;

        public GitHubIntegrationTests()
        {
            gitHubServiceMock = new Mock<IGitHubService>();
            gitCliServiceMock = new Mock<IGitCliService>();
            LoggerMock = new Mock<ILogger<CommitViewerBusiness>>();
            commitViewerBusiness = new CommitViewerBusiness(gitHubServiceMock.Object, gitCliServiceMock.Object, LoggerMock.Object);
        }

        [Fact]
        public async Task GitHubService_Should_Provide_Response_With_Expected_Input_Arguments()
        {
            var gitHubResponseExample = StreamReaderExtensions.ReadFileJsonString("githubResponseExample.txt");

            gitHubServiceMock.Setup(mock => mock.GetGitHubCommits(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(gitHubResponseExample));

            await commitViewerBusiness.GetCommits("octocat", "hello-world", 1, 1);

            //Assert
            gitHubServiceMock.Verify(mock => mock.GetGitHubCommits(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        }
    }
}
