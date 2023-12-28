using System;
using System.Collections.Generic;

namespace TransitLine.Models;

public partial class Delivery
{
    public int IdDelivery { get; set; }

    public DateTime? DepartureDate { get; set; }

    public DateTime? DestinationDate { get; set; }

    public string DeliveryStatus { get; set; }

    public int? IdOrder { get; set; }

    public virtual Order IdOrderNavigation { get; set; }

    public virtual ICollection<TemperatureHumidity> TemperatureHumidities { get; set; } = new List<TemperatureHumidity>();
}
