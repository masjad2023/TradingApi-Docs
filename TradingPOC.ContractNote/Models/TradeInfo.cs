using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPOC.ContractNote.Models
{
    public class TradeInfo
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Date { get; set; }
        public bool isPayable { get; set; }
        public decimal Amount { get; set; }

        public List<TradeDetail> Details { get; set; }

    }

    public class TradeDetail
    {
        public int OrderId { get; set; }
        public DateTime TradeTimeStamp { get; set; }
        public string Script { get; set; }
        public string TradeType { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal BuyAmount { get; set; }
        public decimal SellAmount { get; set; }
    }
}
