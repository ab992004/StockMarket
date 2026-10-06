using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse
    {

        public Guid BuyOrderID { get; set; }
        public string StockSymbol { get; set; }
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double Price { get; set; }
        public double TradeAmount { get; set; }
        public override string ToString()
        {
            return $"BuyOrderID: {BuyOrderID}, StockSymbol: {StockSymbol}, StockName: {StockName}, DateAndTimeOfOrder: {DateAndTimeOfOrder}, Quantity: {Quantity}, Price: {Price}, TradeAmount: {TradeAmount}";
        }
        public override bool Equals(object? obj)
        {
            return obj is BuyOrderResponse response &&
                   BuyOrderID.Equals(response.BuyOrderID) &&
                   StockSymbol == response.StockSymbol &&
                   StockName == response.StockName &&
                   DateAndTimeOfOrder == response.DateAndTimeOfOrder &&
                   Quantity == response.Quantity &&
                   Price == response.Price &&
                   TradeAmount == response.TradeAmount;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(BuyOrderID, StockSymbol, StockName, DateAndTimeOfOrder, Quantity, Price, TradeAmount);
        }

    }
    public static class BuyOrderExtensions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse() { BuyOrderID = buyOrder.BuyOrderID, DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder, Price = buyOrder.Price, Quantity = buyOrder.Quantity, StockName = buyOrder.StockName, StockSymbol = buyOrder.StockSymbol, TradeAmount = buyOrder.Quantity * buyOrder.Price };
        }
    }
}
