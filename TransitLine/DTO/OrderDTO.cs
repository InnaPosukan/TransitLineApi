namespace TransitLine.Dto
{
    public class OrderDTO
    {
        public int IdOrder { get; set; }
        public string DepartureLocation { get; set; }
        public string DestinationLocation { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now; 
        public string OrderStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string Currency { get; set; }
        public float Distance { get; set; }
        public decimal? Amount { get; set; }
        public string UnitsOfMeasurement { get; set; }
        public int IdCargo { get; set; }
        public int DriverUserId { get; set; }
        public decimal? CargoWeight { get; set; }
        public int? NumberUnits { get; set; }

    }

}
