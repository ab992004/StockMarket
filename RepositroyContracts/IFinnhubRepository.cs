using System;
using System.Collections.Generic;
using System.Text;

namespace RepositroyContracts
{
  public interface IFinnhubRepository
  {
    public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSympol);
    public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSympol);
    public Task<List<Dictionary<string, string>>?> GetStocks();
    public Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
  }
}
