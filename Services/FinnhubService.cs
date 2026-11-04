using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepositroyContracts;
using Serilog;
using SerilogTimings;
using ServiceContracts;
using System.Net.Http.Json;
namespace Services
{
  public class FinnhubService : IFinnhubService
  {
    private readonly IFinnhubRepository _finnhubRepository;
    private readonly ILogger<FinnhubService> _logger;
    public FinnhubService(IFinnhubRepository finnhubRepository, ILogger<FinnhubService> logger)
    {
      _finnhubRepository = finnhubRepository;
      _logger = logger;
    }

    public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSympol)
    {
      _logger.LogInformation("GetCompanyProfile method of FinnhubService invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
      Dictionary<string, object>? dict;
      using (Operation.Time("GetCompanyProfile"))
      {
        dict = await _finnhubRepository.GetCompanyProfile(stockSympol);
      }
      return dict;
    }

    public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSympol)
    {
      _logger.LogInformation("GetStockPriceQuote method of FinnhubService invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
      Dictionary<string, object>? dict;
      using (Operation.Time("GetStockPriceQuote"))
      {
        dict = await _finnhubRepository.GetStockPriceQuote(stockSympol);
      }
      return dict;
    }

    public async Task<List<Dictionary<string, string>>?> GetStocks()
    {
      _logger.LogInformation("GetStocks method of FinnhubService invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
      List<Dictionary<string, string>>? dict;
      using (Operation.Time("GetStocks"))
      {
        dict = await _finnhubRepository.GetStocks();
      }
      return dict;
    }

    public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
    {
      _logger.LogInformation("SearchStocks method of FinnhubService invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
      Dictionary<string, object>? dict;
      using (Operation.Time("SearchStocks"))
      {
        dict = await _finnhubRepository.SearchStocks(stockSymbolToSearch);
      }
      return dict;
    }
  }
}
