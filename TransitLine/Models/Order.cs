using System;
using System.Collections.Generic;

namespace TransitLine.Models;

public partial class Order
{
    public int IdOrder { get; set; }

    public string DepartureLocation { get; set; }

    public string DestinationLocation { get; set; }

    public DateTime? CreationDate { get; set; }

    public string OrderStatus { get; set; }

    public float Distance { get; set; }

    public string UnitsOfMeasurement { get; set; }

    public int  IdCargo { get; set; }

    public int IdUser { get; set; }
    public int DriverUserId { get; set; }

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual CargoType IdCargoNavigation { get; set; }

    public virtual User IdUserNavigation { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
