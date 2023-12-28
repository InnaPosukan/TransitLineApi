using System;
using System.Collections.Generic;

namespace TransitLine.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string PhoneNumber { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Role { get; set; }  


    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Truck> Trucks { get; set; } = new List<Truck>();
}
