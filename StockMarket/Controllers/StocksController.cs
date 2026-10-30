using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StockMarket.Models;
using System.Text.Json;

namespace StockMarket.Controllers
{
  [Route("[controller]")]
  public class StocksController : Controller
  {
    private readonly TradingOptions _tradingOptions;
    private readonly IFinnhubService _finnhubService;

    public StocksController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService)
    {
      _tradingOptions = tradingOptions.Value;
      _finnhubService = finnhubService;
    }

    [Route("/")]
    [Route("[action]/{stock?}")]
    [Route("~/[action]/{stock?}")]
    public async Task<IActionResult> Explore(string? stock, string? searchText, bool showAll = false)
    {
      List<Stock> stocks = string.IsNullOrWhiteSpace(searchText)
        ? await GetStocksList(showAll)
        : await SearchStocksList(searchText.Trim());

      ViewBag.stock = stock;
      ViewBag.SearchText = searchText;
      return View(stocks);
    }

    private async Task<List<Stock>> GetStocksList(bool showAll)
    {
      List<Dictionary<string, string>>? stocksDictionary = await _finnhubService.GetStocks();
      if (stocksDictionary is null)
        return new List<Stock>();

      IEnumerable<Dictionary<string, string>> filtered = stocksDictionary;

      if (!showAll && !string.IsNullOrEmpty(_tradingOptions.Top25PopularStocks))
      {
        HashSet<string> popular = _tradingOptions.Top25PopularStocks.Split(',').ToHashSet();
        filtered = filtered.Where(d => d.TryGetValue("symbol", out var sym) && popular.Contains(sym));
      }

      return filtered
        .Where(d => d.ContainsKey("symbol") && d.ContainsKey("description"))
        .Select(d => new Stock { StockName = d["description"], StockSymbol = d["symbol"] })
        .ToList();
    }

    private async Task<List<Stock>> SearchStocksList(string searchText)
    {
      Dictionary<string, object>? response = await _finnhubService.SearchStocks(searchText);

      if (response is null
          || !response.TryGetValue("result", out object? resultObj)
          || resultObj is not JsonElement { ValueKind: JsonValueKind.Array } resultArray)
        return new List<Stock>();

      List<Stock> stocks = new List<Stock>();
      foreach (JsonElement item in resultArray.EnumerateArray())
      {
        if (item.TryGetProperty("symbol", out JsonElement sym)
            && item.TryGetProperty("description", out JsonElement desc))
        {
          stocks.Add(new Stock { StockSymbol = sym.GetString(), StockName = desc.GetString() });
        }
      }
      return stocks;
    }
  }
}