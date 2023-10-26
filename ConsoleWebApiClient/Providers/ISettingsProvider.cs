using ConsoleWebApiClient.Entities;

namespace ConsoleWebApiClient.Providers
{
    internal interface ISettingsProvider
    {
        Settings Provide();
    }
}
