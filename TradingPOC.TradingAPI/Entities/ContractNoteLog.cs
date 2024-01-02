using System;
using System.Collections.Generic;

namespace TradingPOC.TradingAPI.Entities;

public partial class ContractNoteLog
{
    public int IdcontractNoteLog { get; set; }

    public int? UserId { get; set; }

    public DateTime? LogDatetime { get; set; }
}
