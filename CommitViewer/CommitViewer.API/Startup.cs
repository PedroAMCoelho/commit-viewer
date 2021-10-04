using CommitViewer.IoC.Services;
using CommitViewer.Services.GitHubService.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using CommitViewer.Shared.Options.Extensions;
using CommitViewer.IoC.Business;
using CommitViewer.Business.Mappings;
using Microsoft.Extensions.Logging;
using Polly.Extensions.Http;
using System;
using Polly;

namespace CommitViewer.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<HttpClient>();

            services.AddLogging(opt =>
            {
                opt.AddConsole();
            });

            services.AddOptionsConfig<GitHubOptions>(Configuration, true);

            services.AddAutoMapper(new[]
            {
                typeof(CommitViewerMapping)
            });

            // Dependency Injection Containers
            GitHubServiceCollectionExtensions.InitializeGitHubApplicationServices(services, Configuration);
            GitCliServiceCollectionExtensions.InitializeGitHubApplicationServices(services, Configuration);
            CommitViewerServiceCollectionExtensions.InitializeCommitViewerApplicationServices(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
