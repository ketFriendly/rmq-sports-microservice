using System;
using System.Collections.Generic;

namespace ProducerService1.DB.Models;

public partial class Sport
{
    public int Id { get; set; }

    public string Sport1 { get; set; } = null!;

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}
