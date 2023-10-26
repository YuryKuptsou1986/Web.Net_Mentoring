using ConsoleWebApiClient.Entities;
using Microsoft.Extensions.Configuration;

namespace ConsoleWebApiClient.Providers
{
    internal class SettingsProvider : ISettingsProvider
    {
        private readonly Settings _settings;
        private const string SettingsFileName = "appsettings.json";
        private const string SettingsSection = "Settings";

        public SettingsProvider(IConfigurationBuilder configurationBuilder) 
        {
            IConfigurationRoot config = configurationBuilder
                    .AddJsonFile(SettingsFileName)
                    .Build();

            _settings = config.GetRequiredSection(SettingsSection).Get<Settings>();
            ValidateSettings(_settings);
        }

        public Settings Provide()
        {
            return _settings;
        }

        private void ValidateSettings(Settings settings)
        {
            if(settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }

            ValidateSetting(settings.WebApiBaseUrl, nameof(settings.WebApiBaseUrl));
            ValidateSetting(settings.GetProductsEndpoint, nameof(settings.GetProductsEndpoint));
            ValidateSetting(settings.GetCategoriesEndpoint, nameof(settings.GetCategoriesEndpoint));
        }

        private void ValidateSetting(string value, string fieldName)
        {
            if(string.IsNullOrEmpty(value)) {
                throw new ArgumentNullException(fieldName, "Settings can not be empty.");
            }
        }
    }
}
