using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositroyContracts
{
  public interface IStocksRepository
  {

    Task<BuyOrder> AddBuyOrder(BuyOrder buyOrder);

    Task<SellOrder> AddSellOrder(SellOrder sellOrder);

    Task<List<BuyOrder>> GetBuyOrders();

    Task<List<SellOrder>> GetSellOrders();
  }
}
