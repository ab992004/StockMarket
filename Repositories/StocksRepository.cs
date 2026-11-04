using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositroyContracts;
namespace Repositories
{
  public class StocksRepository : IStocksRepository
  {
    private readonly StockMarketDbContext _dbContext;
    private readonly ILogger<StocksRepository> _logger;
    public StocksRepository(StockMarketDbContext db, ILogger<StocksRepository> logger)
    {
      _dbContext = db;
      _logger = logger;
    }
    public async Task<BuyOrder> AddBuyOrder(BuyOrder buyOrder)
    {
      _logger.LogInformation("AddBuyOrder method of StocksRepository invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
      _dbContext.BuyOrders.Add(buyOrder);
      await _dbContext.SaveChangesAsync();
      return buyOrder;
    }

    public async Task<SellOrder> AddSellOrder(SellOrder sellOrder)
    {
      _logger.LogInformation("AddSellOrder method of StocksRepository invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
      _dbContext.SellOrders.Add(sellOrder);
      await _dbContext.SaveChangesAsync();
      return sellOrder;
    }

    public async Task<List<BuyOrder>> GetBuyOrders()
    {
      _logger.LogInformation("GetBuyOrders method of StocksRepository invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
      List<BuyOrder> buyOrders = await _dbContext.BuyOrders.ToListAsync();
      return buyOrders;
    }

    public async Task<List<SellOrder>> GetSellOrders()
    {
      _logger.LogInformation("GetSellOrders method of StocksRepository invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
      List<SellOrder> sellOrder = await _dbContext.SellOrders.ToListAsync();
      return sellOrder;
    }
  }
}
