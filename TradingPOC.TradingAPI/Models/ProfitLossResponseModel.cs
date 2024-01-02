using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Crypto.Generators;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TradingPOC.TradingAPI.Models
{
    public class ProfitLossResponseModel
    {
        public string ScriptName { get; set; }
        public int Quantity { get; set; }
        public DateTime SellDate { get; set; }
        public DateTime BuyDate { get; set; }
        public decimal AvgSellRate { get; set; }
        public decimal TotalSellValue { get; set; }
        public decimal AvgBuyRate { get; set; }
        public decimal TotalBuyValue { get; set; }
        public decimal RealizedGainOrLoss { get; set; }
        public string LongOrShort { get; set; }

    }
}
