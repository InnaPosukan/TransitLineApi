namespace TransitLine.DTO
{
    public class AssignDriverDTO
    {
        public int IdOrder { get; set; }
        public int DriverUserId { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? DestinationDate { get; set; }
    }

}
