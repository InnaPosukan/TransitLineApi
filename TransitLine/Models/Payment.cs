using System;
using System.Collections.Generic;

namespace TransitLine.Models;

public partial class Payment
{
    public int IdPayment { get; set; }

    public string PaymentStatus { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string PaymentMethod { get; set; }

    public string Currency { get; set; }

    public int? IdOrder { get; set; }

    public virtual Order IdOrderNavigation { get; set; }
}
