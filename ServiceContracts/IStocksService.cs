using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContracts
{
    public interface IStocksService
    {
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

        Task<List<BuyOrderResponse>> GetBuyOrders();

        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
