﻿namespace TradingPOC.Data.API.Models
{
    public class PriceAPIResponseModel
    {
        public string symbol { get; set; }
        public string identifier { get; set; }
        public double open { get; set; }
        public double dayHigh { get; set; }
        public double dayLow { get; set; }
        public double lastPrice { get; set; }
        public double previousClose { get; set; }
        public double change { get; set; }
        public double pChange { get; set; }
        public double yearHigh { get; set; }
        public double yearLow { get; set; }
        public int totalTradedVolume { get; set; }
        public double totalTradedValue { get; set; }
        public string lastUpdateTime { get; set; }
        public double perChange365d { get; set; }
        public double perChange30d { get; set; }
    }
}
