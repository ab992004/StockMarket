using Entities;
using Microsoft.EntityFrameworkCore;
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
        private readonly StockMarketDbContext _db;


        /// <summary>
        /// Constructor of StocksService class that executes when a new object is created for the class
        /// </summary>
        public StocksService(StockMarketDbContext dbContext)
        {
            _db = dbContext;
        }
        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest is null) throw new ArgumentNullException("BuyOrderRequest can't be null");
            ValidationHelper.ModelValidation(buyOrderRequest);
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();
            await _db.BuyOrders.AddAsync(buyOrder);
            await _db.SaveChangesAsync();
            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest is null) throw new ArgumentNullException("SellOrderRequest can't be null");
            ValidationHelper.ModelValidation(sellOrderRequest);
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();
            await _db.SellOrders.AddAsync(sellOrder);
            await _db.SaveChangesAsync();
            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            var buyOrders = await _db.BuyOrders.ToListAsync();
            return  buyOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).Select(temp => temp.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            var sellOrders = await _db.SellOrders.ToListAsync();
            return sellOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).Select(temp => temp.ToSellOrderResponse()).ToList();
        }
    }
}
