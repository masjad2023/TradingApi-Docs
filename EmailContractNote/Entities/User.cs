using System;
using System.Collections.Generic;

namespace TradingPOC.ContractNote.Entities;

public partial class User
{
    public int Id { get; set; }

    public string EmailId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<Trade> Trades { get; set; } = new List<Trade>();
}
