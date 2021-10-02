using CommitViewer.Business.CommitViewer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommitViewer.IoC.Business
{
    public class CommitViewerServiceCollectionExtensions
    {
        public static void InitializeCommitViewerApplicationServices(IServiceCollection services, IConfiguration configuration)
        {           
            services.AddScoped<ICommitViewerBusiness, CommitViewerBusiness>();
        }
    }
}
