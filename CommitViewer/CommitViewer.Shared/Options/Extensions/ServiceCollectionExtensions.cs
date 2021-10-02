using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CommitViewer.Shared.Options.Extensions
{
    /// <summary>
    /// The service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the options config.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="allowSettingsRealTimeUpdate">If true, allow settings real time update.</param>
        public static void AddOptionsConfig<T>(this IServiceCollection services,
            IConfiguration configuration, bool allowSettingsRealTimeUpdate = false)
            where T : AppSettingsOptions, new()
        {
            IConfiguration appSettingsConfig = configuration.SafeGetConfigSection<T>();
            services.Configure<T>(appSettingsConfig);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            OptionsConfig<T>.Initialize(serviceProvider, allowSettingsRealTimeUpdate);
        }
    }
}
