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
    private readonly ILogger<StocksController> _logger;

    public StocksController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService, ILogger<StocksController> logger)
    {
      _tradingOptions = tradingOptions.Value;
      _finnhubService = finnhubService;
      _logger = logger;
    }

    [Route("/")]
    [Route("[action]/{stock?}")]
    [Route("~/[action]/{stock?}")]
    public async Task<IActionResult> Explore(string? stock, string? searchText, bool showAll = false)
    {
      _logger.LogInformation("Explore action called with stock: {Stock}, searchText: {SearchText}, showAll: {ShowAll}", stock, searchText, showAll);
      List<Stock> stocks = string.IsNullOrWhiteSpace(searchText)
        ? await GetStocksList(showAll)
        : await SearchStocksList(searchText.Trim());

      ViewBag.stock = stock;
      ViewBag.SearchText = searchText;
      return View(stocks);
    }

    private async Task<List<Stock>> GetStocksList(bool showAll)
    {
      _logger.LogInformation("GetStocksList called with showAll: {ShowAll}", showAll);
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
      _logger.LogInformation("SearchStocksList called with searchText: {SearchText}", searchText);
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