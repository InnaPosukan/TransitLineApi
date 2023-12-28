using System;
using System.Collections.Generic;

namespace TransitLine.Models;

public partial class Truck
{
    public int IdTruck { get; set; }

    public decimal? Capacity { get; set; }

    public decimal? Height { get; set; }

    public decimal? Length { get; set; }

    public string Model { get; set; }

    public string CarNumber { get; set; }

    public int? UserId { get; set; }

    public virtual User User { get; set; }
}
