namespace ConsoleWebApiClient.Providers
{
    internal interface IHttpClientProvider
    {
        HttpClient Provide(string webApiUrl);
    }
}
