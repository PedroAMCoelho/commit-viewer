﻿using CommitViewer.Services.GitCliService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommitViewer.IoC.Services
{
    public class GitCliServiceCollectionExtensions
    {
        public static void InitializeGitHubApplicationServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IGitCliService, GitCliService>();
        }
    }
}
