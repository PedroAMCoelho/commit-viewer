using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace CommitViewer.Shared.Options
{
    public static class OptionsConfig<T>
        where T : AppSettingsOptions, new()
    {
        private static IOptions<T> _options;
        private static IOptionsMonitor<T> _optionsMonitor;

        private static bool? _allowSettingsRealTimeUpdate = null;

        /// <summary>
        /// Gets the options.
        /// </summary>
        public static T Options
        {
            get
            {
                if (_allowSettingsRealTimeUpdate.Value)
                    return GetMonitorOptions();
                else
                    return GetOptions();
            }
        }

        /// <summary>
        /// Initializes the.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="allowSettingsRealTimeUpdate">If true, allow settings real time update.</param>
        public static void Initialize(IServiceProvider serviceProvider, bool allowSettingsRealTimeUpdate)
        {
            if (_allowSettingsRealTimeUpdate.HasValue)
                throw new InvalidOperationException(
                            "The OptionConfig has already been configured. It is not allowed to configure this twice.");

            _allowSettingsRealTimeUpdate = allowSettingsRealTimeUpdate;

            if (allowSettingsRealTimeUpdate)
                InitializeRealTimeUpdate(serviceProvider);
            else
                InitializeSingleton(serviceProvider);
        }

        private static void InitializeRealTimeUpdate(IServiceProvider serviceProvider)
        {
            if (_optionsMonitor == null)
                _optionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<T>>();
        }

        private static void InitializeSingleton(IServiceProvider serviceProvider)
        {
            if (_options == null)
                _options = serviceProvider.GetRequiredService<IOptions<T>>();
        }

        private static T GetMonitorOptions()
        {
            if (_optionsMonitor == null)
                throw new InvalidOperationException(
                    "The OptionConfig was not configured correctly in the startup.");

            return _optionsMonitor.CurrentValue;
        }

        private static T GetOptions()
        {
            if (_options == null)
                throw new InvalidOperationException(
                    "The OptionConfig was not configured correctly in the startup.");

            return _options.Value;
        }
    }
}
