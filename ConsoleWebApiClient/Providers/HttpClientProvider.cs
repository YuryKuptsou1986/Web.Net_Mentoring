using System.Net.Http.Headers;
using System.Net.Mime;

namespace ConsoleWebApiClient.Providers
{
    internal class HttpClientProvider : IHttpClientProvider
    {
        public HttpClient Provide(string webApiUrl)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(webApiUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            return httpClient;
        }
    }
}
