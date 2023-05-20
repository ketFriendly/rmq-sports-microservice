using System;
using System.Collections.Generic;

namespace ProducerService1.DB.Models;

public partial class Match
{
    public int Id { get; set; }

    public int? SportId { get; set; }

    public string? Name { get; set; }

    public DateTime? StartTime { get; set; }

    public virtual ICollection<Market> Markets { get; set; } = new List<Market>();

    public virtual Sport? Sport { get; set; }
}
