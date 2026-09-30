using Microsoft.Extensions.Configuration;
using ServiceContracts;
using System.Net.Http.Json;
namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSympol)
        {
            using(HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol=AAPL&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };
                HttpResponseMessage httpResponse= await httpClient.SendAsync(httpRequest);
                var dict = await httpResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>()?? throw new InvalidOperationException("No response from finnhub server");

                if (dict.TryGetValue("error", out var error))
                    throw new InvalidOperationException(Convert.ToString(error));

                return dict;
            }
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSympol)
        {
            using(HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSympol}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };
                HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);
                var dict = await httpResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>()?? throw new InvalidOperationException("No response from finnhub server");

                if (dict.TryGetValue("error", out var error))
                    throw new InvalidOperationException(Convert.ToString(error));

                return dict;
            }
        }
    }
}
