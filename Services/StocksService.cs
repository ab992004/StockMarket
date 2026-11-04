using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositroyContracts;
using Serilog;
using SerilogTimings;
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
    private readonly ILogger<StocksService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;
    /// <summary>
    /// Constructor of StocksService class that executes when a new object is created for the class
    /// </summary>
    public StocksService(IStocksRepository stocksRepository, ILogger<StocksService> logger, IDiagnosticContext diagnosticContext)
    {
      _stocksRepository = stocksRepository;
      _logger = logger;
      _diagnosticContext = diagnosticContext;
    }
    public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
    {
      _logger.LogInformation("CreateBuyOrder method of StocksService invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
      if (buyOrderRequest is null) throw new ArgumentNullException("BuyOrderRequest can't be null");
      ValidationHelper.ModelValidation(buyOrderRequest);
      BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
      buyOrder.BuyOrderID = Guid.NewGuid();
      await _stocksRepository.AddBuyOrder(buyOrder);

      return buyOrder.ToBuyOrderResponse();
    }

    public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
    {
      _logger.LogInformation("CreateSellOrder method of StocksService invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
      if (sellOrderRequest is null) throw new ArgumentNullException("SellOrderRequest can't be null");
      ValidationHelper.ModelValidation(sellOrderRequest);
      SellOrder sellOrder = sellOrderRequest.ToSellOrder();
      sellOrder.SellOrderID = Guid.NewGuid();
      await _stocksRepository.AddSellOrder(sellOrder);
      return sellOrder.ToSellOrderResponse();
    }

    public async Task<List<BuyOrderResponse>> GetBuyOrders()
    {
      List<BuyOrder> buyOrders;
      _logger.LogInformation("GetBuyOrders method of StocksService invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
      using (Operation.Time("GetBuyOrders"))
      {
       buyOrders= await _stocksRepository.GetBuyOrders();
      }
      _diagnosticContext.Set("BuyOrdersCount", buyOrders.Count);
      return buyOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).Select(temp => temp.ToBuyOrderResponse()).ToList();
    }

    public async Task<List<SellOrderResponse>> GetSellOrders()
    {
      _logger.LogInformation("GetSellOrders method of StocksService invoked at {DT}", DateTime.UtcNow.ToLongTimeString());
      List<SellOrder> sellOrders;
      using (Operation.Time("GetSellOrders"))
      {
        sellOrders = await _stocksRepository.GetSellOrders();
      }
      _diagnosticContext.Set("SellOrdersCount", sellOrders.Count);
      return sellOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).Select(temp => temp.ToSellOrderResponse()).ToList();
    }
  }
}
