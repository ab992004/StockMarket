using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositroyContracts;
namespace Repositories
{
  public class StocksRepository : IStocksRepository
  {
    private readonly StockMarketDbContext _dbContext;
    public StocksRepository(StockMarketDbContext db)
    {
      _dbContext = db;
    }
    public async Task<BuyOrder> AddBuyOrder(BuyOrder buyOrder)
    {
      _dbContext.BuyOrders.Add(buyOrder);
      await _dbContext.SaveChangesAsync();
      return buyOrder;
    }

    public async Task<SellOrder> AddSellOrder(SellOrder sellOrder)
    {
      _dbContext.SellOrders.Add(sellOrder);
      await _dbContext.SaveChangesAsync();
      return sellOrder;
    }

    public async Task<List<BuyOrder>> GetBuyOrders()
    {
      List<BuyOrder> buyOrders = await _dbContext.BuyOrders.ToListAsync();
      return buyOrders;
    }

    public async Task<List<SellOrder>> GetSellOrders()
    {
      List<SellOrder> sellOrder = await _dbContext.SellOrders.ToListAsync();
      return sellOrder;
    }
  }
}
