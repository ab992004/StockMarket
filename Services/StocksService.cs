using Entities;
using Microsoft.EntityFrameworkCore;
using RepositroyContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
namespace Services
{
  public class StocksService : IStocksService
  {
    //private field
    private readonly IStocksRepository _stocksRepository;
    /// <summary>
    /// Constructor of StocksService class that executes when a new object is created for the class
    /// </summary>
    public StocksService(IStocksRepository stocksRepository)
    {
      _stocksRepository = stocksRepository;
    }
    public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
    {
      if (buyOrderRequest is null) throw new ArgumentNullException("BuyOrderRequest can't be null");
      ValidationHelper.ModelValidation(buyOrderRequest);
      BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
      buyOrder.BuyOrderID = Guid.NewGuid();
      await _stocksRepository.AddBuyOrder(buyOrder);

      return buyOrder.ToBuyOrderResponse();
    }

    public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
    {
      if (sellOrderRequest is null) throw new ArgumentNullException("SellOrderRequest can't be null");
      ValidationHelper.ModelValidation(sellOrderRequest);
      SellOrder sellOrder = sellOrderRequest.ToSellOrder();
      sellOrder.SellOrderID = Guid.NewGuid();
      await _stocksRepository.AddSellOrder(sellOrder);
      return sellOrder.ToSellOrderResponse();
    }

    public async Task<List<BuyOrderResponse>> GetBuyOrders()
    {
      var buyOrders = await _stocksRepository.GetBuyOrders();
      return buyOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).Select(temp => temp.ToBuyOrderResponse()).ToList();
    }

    public async Task<List<SellOrderResponse>> GetSellOrders()
    {
      var sellOrders = await _stocksRepository.GetSellOrders();
      return sellOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).Select(temp => temp.ToSellOrderResponse()).ToList();
    }
  }
}
