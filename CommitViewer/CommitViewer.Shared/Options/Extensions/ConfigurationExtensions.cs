using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace CommitViewer.Shared.Options.Extensions
{
    /// <summary>
    /// The configuration extensions.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Safes the get.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>A T.</returns>
        public static T SafeGet<T>(this IConfiguration configuration)
        {
            string typeName = typeof(T).Name;

            if (configuration.GetChildren().Any(item => item.Key == typeName))
                configuration = configuration.GetSection(typeName);

            T model = configuration.Get<T>();

            if (model == null)
                throw new InvalidOperationException(
                    $"Configuration item for type {typeof(T).FullName} not found.");

            return model;
        }

        /// <summary>
        /// Safes the get config section.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>An IConfiguration.</returns>
        public static IConfiguration SafeGetConfigSection<T>(this IConfiguration configuration)
        {
            string typeName = typeof(T).Name;

            if (configuration.GetChildren().Any(item => item.Key == typeName))
                configuration = configuration.GetSection(typeName);

            T model = configuration.Get<T>();

            if (model == null)
                throw new InvalidOperationException(
                    $"Configuration item for type {typeof(T).FullName} not found.");

            return configuration;
        }
    }
}
