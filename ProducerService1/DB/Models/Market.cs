using System;
using System.Collections.Generic;

namespace ProducerService1.DB.Models;

public partial class Market
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public int MarketId { get; set; }

    public decimal Value { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual Match Match { get; set; } = null!;
}
