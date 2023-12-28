using System;
using System.Collections.Generic;

namespace TransitLine.Models;

public partial class TemperatureHumidity
{
    public int IdTemperatureHumidity { get; set; }

    public DateTime? Timestamp { get; set; }

    public decimal? Temperature { get; set; }

    public decimal? Humidity { get; set; }

    public int? IdDelivery { get; set; }

    public virtual Delivery IdDeliveryNavigation { get; set; }
}
