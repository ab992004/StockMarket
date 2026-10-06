using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class SellOrder
    {
        
        public Guid SellOrderID { get; set; }
        [Required]
        public string? StockSymbol { get; set; }
        [Required]
        public string? StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000, ErrorMessage = "The quantity must be between 1 and 100000.")]
        public uint Quantity { get; set; }
        [Range(1, 10000, ErrorMessage = "The maximum price of stock is 10000. Minimum is 1.")]
        public double Price { get; set; }

    }
}
