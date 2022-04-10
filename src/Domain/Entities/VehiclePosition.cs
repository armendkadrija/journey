
using NetTopologySuite.Geometries;

namespace Journey.Domain.Entities
{
    public class VehiclePosition
    {
        public int Id { get; set; }
        public string RouteNumber { get; set; }
        public string Direction { get; set; }
        public int Operator { get; set; }
        public DateTime TimeStamp { get; set; }
        public int VehicleNumber { get; set; }
        public double Speed { get; set; }
        public int HeadingDegree { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Point Location { get; set; }
        public double Acceleration { get; set; }
        public Boolean DoorStatus { get; set; }
        public string LocationSource { get; set; }
        public string Route { get; set; }
        public int Occupants { get; set; }
    }

}
