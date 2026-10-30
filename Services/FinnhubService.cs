using Microsoft.Extensions.Configuration;
using RepositroyContracts;
using ServiceContracts;
using System.Net.Http.Json;
namespace Services
{
  public class FinnhubService : IFinnhubService
  {
    private readonly IFinnhubRepository _finnhubRepository;

    public FinnhubService(IFinnhubRepository finnhubRepository)
    {
      _finnhubRepository = finnhubRepository;
    }

    public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSympol)
    {
      var dict = _finnhubRepository.GetCompanyProfile(stockSympol);
      return dict;
    }

    public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSympol)
    {
      var dict = _finnhubRepository.GetStockPriceQuote(stockSympol);
      return dict;
    }

    public Task<List<Dictionary<string, string>>?> GetStocks()
    {
      var dict = _finnhubRepository.GetStocks();
      return dict;
    }

    public Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
    {
      var dict = _finnhubRepository.SearchStocks(stockSymbolToSearch);
      return dict;
    }
  }
}
