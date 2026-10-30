using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using RepositroyContracts;
namespace Repositories
{
  public class FinnhubRepository: IFinnhubRepository
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    public FinnhubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
      _httpClientFactory = httpClientFactory;
      _configuration = configuration;
    }

    public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSympol)
    {
      string? token = _configuration["FinnhubToken"];

      if (string.IsNullOrEmpty(token))
        throw new InvalidOperationException("Finnhub token isn't configured");

      HttpClient httpClient = _httpClientFactory.CreateClient();

      using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/stock/profile2?symbol={Uri.EscapeDataString(stockSympol)}");
      httpRequest.Headers.Add("X-Finnhub-Token", token);

      HttpResponseMessage httpResponse =await httpClient.SendAsync(httpRequest);

      if (!httpResponse.IsSuccessStatusCode)
      {
        string body = await httpResponse.Content.ReadAsStringAsync();
        throw new InvalidOperationException($"Finnhub returned {(int)httpResponse.StatusCode}:{body}");
      }
      return await httpResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>() ?? throw new InvalidOperationException("Empty response from Finnhub server");
    }

    public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSympol)
    {
      string? token = _configuration["FinnhubToken"];

      if (string.IsNullOrEmpty(token))
        throw new InvalidOperationException("Finnhub token isn't configured");

      HttpClient httpClient = _httpClientFactory.CreateClient();

      using var httpRequest = new HttpRequestMessage(HttpMethod.Get,$"https://finnhub.io/api/v1/quote?symbol={Uri.EscapeDataString(stockSympol)}");

      httpRequest.Headers.Add("X-Finnhub-Token", token);

      HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);

      if (!httpResponse.IsSuccessStatusCode)
      {
        string body = await httpResponse.Content.ReadAsStringAsync();
        throw new InvalidOperationException($"Finnhub returned {(int) httpResponse.StatusCode}:{body}");

      }

      return await httpResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>() ?? throw new InvalidOperationException("Empty response from Finnhub server."); 
    }

    public async Task<List<Dictionary<string, string>>?> GetStocks()
    {
      string? token = _configuration["FinnhubToken"];
      if (string.IsNullOrEmpty(token))
        throw new InvalidOperationException("Finnhub Token isn't configured");

      HttpClient httpClient = _httpClientFactory.CreateClient();

      using var httpRequest = new HttpRequestMessage(HttpMethod.Get, "https://finnhub.io/api/v1/stock/symbol?exchange=US");

      httpRequest.Headers.Add("X-Finnhub-Token", token);

      using HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);

      if (!httpResponse.IsSuccessStatusCode)
      {
        string body = await httpResponse.Content.ReadAsStringAsync();
        throw new InvalidOperationException($"Finnhub returned {(int)httpResponse.StatusCode}:{body}");
      }


      return await httpResponse.Content.ReadFromJsonAsync<List<Dictionary<string, string>>>()
        ?? throw new InvalidOperationException("Empty response from Finnhub.");
    }

    public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
    {
      string? token = _configuration["FinnhubToken"];

      if (string.IsNullOrEmpty(token))
        throw new InvalidOperationException("Finnhub token isn't configured");

      HttpClient httpClient = _httpClientFactory.CreateClient();

      using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/search?q={Uri.EscapeDataString(stockSymbolToSearch)}&exchange=US");
      httpRequest.Headers.Add("X-Finnhub-Token", token);
      HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);

      if (!httpResponse.IsSuccessStatusCode)
      {
        string body = await httpResponse.Content.ReadAsStringAsync();
        throw new InvalidOperationException($"FinnHub Returned {(int)httpResponse.StatusCode}:{body}");
      }
      return await httpResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>()
        ?? throw new InvalidOperationException("Empty response from Finnhub.");
    }
  }
}
