public class OrderUpdateDTO
{
    public int OrderId { get; set; }

    public string DepartureLocation { get; set; }

    public string DestinationLocation { get; set; }

    public string OrderStatus { get; set; }

    public int? DriverUserId { get; set; }
}
