using System.Text.Json.Serialization;
using AutoMapper;
using Journey.Application.Common.Mappings;
using Journey.Domain.Entities;
using NetTopologySuite.Geometries;

namespace Application.Common.Models
{
    public class VehiclePositionPayload : IMapFrom<VehiclePosition>
    {
        [JsonPropertyName("desi")]
        public string RouteNumber { get; set; }

        [JsonPropertyName("dir")]
        public string Direction { get; set; }

        [JsonPropertyName("oper")]
        public int Operator { get; set; }

        [JsonPropertyName("veh")]
        public int VehicleNumber { get; set; }

        [JsonPropertyName("tst")]
        public DateTime TimeStamp { get; set; }

        [JsonPropertyName("spd")]
        public double Speed { get; set; }

        [JsonPropertyName("hdg")]
        public int HeadingDegree { get; set; }

        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonPropertyName("long")]
        public double Longitude { get; set; }

        [JsonPropertyName("acc")]
        public double Acceleration { get; set; }

        [JsonPropertyName("drs")]
        public int DoorStatus { get; set; }

        [JsonPropertyName("loc")]
        public string LocationSource { get; set; }

        [JsonPropertyName("route")]
        public string Route { get; set; }

        [JsonPropertyName("occu")]
        public int Occupants { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<VehiclePositionPayload, VehiclePosition>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new Point(src.Longitude, src.Latitude)));
        }
    }
}