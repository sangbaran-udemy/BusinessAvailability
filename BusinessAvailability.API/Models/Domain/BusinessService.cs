namespace BusinessAvailability.API.Models.Domain
{
    public class BusinessService
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Location { get; set; }
        public DateTime OpenTime  { get; set; }
        public DateTime CloseTime { get; set; }
    }
}
