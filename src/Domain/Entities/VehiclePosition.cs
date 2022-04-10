namespace Journey.Domain.Entities
{
    public class VehiclePosition
    {
        public int Id { get; set; }
        public string RouteNumber { get; set; }

        // dir
        public string Direction { get; set; }

        // oper
        public int Operator { get; set; }

        public DateTime TimeStamp { get; set; }

        // veh
        public int VehicleNumber { get; set; }

        //spd
        public double Speed { get; set; }

        //hdg
        public int HeadingDegree { get; set; }

        //lat
        public double Latitude { get; set; }

        //long
        public double Longitude { get; set; }
        // acc
        public double Acceleration { get; set; }

        //drs
        public Boolean DoorStatus { get; set; }

        //loc
        public string LocationSource { get; set; }

        //route
        public string Route { get; set; }

        //occ
        public int Occupants { get; set; }
    }

}
