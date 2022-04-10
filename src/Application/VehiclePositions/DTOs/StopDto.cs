namespace Application.VehiclePositions.DTOs
{
    public class StopDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Zone { get; set; }
    }
}