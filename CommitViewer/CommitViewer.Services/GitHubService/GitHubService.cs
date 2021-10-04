using CommitViewer.Services.GitHubService.Exceptions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommitViewer.Services.GitHubService
{
    public class GitHubService : IGitHubService
    {
        private readonly HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubService"/> class.
        /// </summary>
        public GitHubService(
            IHttpClientFactory clientFactory)
        {
            this.client = clientFactory.CreateClient(nameof(GitHubService));
        }

        public async Task<string> GetGitHubCommits(string owner, string repository, int page, int page_results)
        {
            string endpoint = $"/repos/{owner}/{repository}/commits?page={page}&per_page={page_results}";

            var res = await client.GetAsync(endpoint);

            await ValidateResponse(res, endpoint, "GET");
            
            return await res.Content.ReadAsStringAsync();
        }

        /// ToDo: Make this method more abstract, maybe in a middleware.
        private async Task ValidateResponse(HttpResponseMessage response, string httpVerb, string endpoint)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new GitHubException($"GitHub authorization issue. Please check your credentials", null, HttpStatusCode.Unauthorized);
            }

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new GitHubException($"Error: Message: {await response.Content.ReadAsStringAsync()}", new GitHubException(), HttpStatusCode.Forbidden);
            }

            if (response.StatusCode.Equals(HttpStatusCode.InternalServerError))
            {
                throw new GitHubException($"Error: Message: {await response.Content.ReadAsStringAsync()}", new GitHubException(), HttpStatusCode.BadRequest);
            }

            if (response.StatusCode.Equals(HttpStatusCode.NotFound))
            {
                throw new GitHubException($"Error: Message: {await response.Content.ReadAsStringAsync()}", new GitHubException(), HttpStatusCode.NotFound);
            }

            if (HttpStatusCode.BadRequest == response.StatusCode)
            {
                throw new GitHubException($"Error: {httpVerb} - Endpoint: {endpoint}. Message: {await response.Content.ReadAsStringAsync()}", new GitHubException(), HttpStatusCode.BadRequest);
            }
        }
    }
}
