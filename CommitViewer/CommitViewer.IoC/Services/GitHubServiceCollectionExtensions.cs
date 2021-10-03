using CommitViewer.Services.GitHubService;
using CommitViewer.Services.GitHubService.Options;
using CommitViewer.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

namespace CommitViewer.IoC.Services
{
    public class GitHubServiceCollectionExtensions
    {
        public static void InitializeGitHubApplicationServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IGitHubService, GitHubService>();

            services.AddHttpClient(nameof(GitHubService), c => {
                c.BaseAddress = new Uri(OptionsConfig<GitHubOptions>.Options.APIBaseUrl);
                c.Timeout = TimeSpan.FromMilliseconds(OptionsConfig<GitHubOptions>.Options.APITimeout);
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                c.DefaultRequestHeaders.Add("User-Agent", nameof(CommitViewer));
                c.DefaultRequestHeaders.Add("cache-control", "no-cache");
            });
        }
    }
}
