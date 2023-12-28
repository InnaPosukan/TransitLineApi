using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransitLine.Models;

public partial class CargoType
{
    public int IdCargo { get; set; }

    public decimal? CargoWeight { get; set; }

    public int? NumberUnits { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
