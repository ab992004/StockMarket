namespace ServiceContracts
{
  public interface IFinnhubService
  {
    Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSympol);
    Task<Dictionary<string, object>?> GetCompanyProfile(string stockSympol);
    Task<List<Dictionary<string, string>>?> GetStocks();
    Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
  }
}
