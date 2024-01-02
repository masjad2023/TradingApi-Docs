using System;
using System.Collections.Generic;

namespace TradingPOC.ContractNote.Entities;

public partial class Trade
{
    public string TradeType { get; set; } = null!;

    public DateTime TradeTimeStamp { get; set; }

    public int? UserId { get; set; }

    public string Symbol { get; set; } = null!;

    public decimal TradePrice { get; set; }

    public int TradeQuantity { get; set; }

    public int OrderId { get; set; }

    public virtual User? User { get; set; }
}
