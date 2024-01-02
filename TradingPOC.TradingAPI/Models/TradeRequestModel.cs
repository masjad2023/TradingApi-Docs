namespace TradingPOC.TradingAPI.Models
{
    public class TradeRequestModel
    {
        public int UserId { get; set; }
        public string Type { get; set; }
        public string Symbol { get; set; }
        public double Cmp { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
