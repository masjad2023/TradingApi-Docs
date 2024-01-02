namespace TradingPOC.TradingAPI.Models
{
    public class TradeResponseModel
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ScriptName { get; set; }
        public string TradeType {  get; set; }
        public string UserEmail {  get; set; }
    }
}
