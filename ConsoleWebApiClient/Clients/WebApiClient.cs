using ConsoleWebApiClient.Providers;

namespace ConsoleWebApiClient.Clients
{
    internal class WebApiClient : IWebApiClient
    {
        private readonly HttpClient _httpClient;

        public WebApiClient(IHttpClientProvider httpClientProvider, ISettingsProvider settingsProvider) 
        {
            var baseUrl = settingsProvider.Provide().WebApiBaseUrl;
            if (baseUrl == null) {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            _httpClient = httpClientProvider.Provide(baseUrl);
        }

        public async Task<IEnumerable<TEntity>> GetEntitiesAsync<TEntity>(string path) where TEntity : class
        {
            HttpResponseMessage response = await _httpClient.GetAsync(path);
            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<IEnumerable<TEntity>>();
            }
            return null;
        }
    }
}
