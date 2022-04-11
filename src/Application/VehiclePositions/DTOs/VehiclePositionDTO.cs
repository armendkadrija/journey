using AutoMapper;
using Journey.Application.Common.Mappings;
using Journey.Domain.Entities;

namespace Application.VehiclePositions.DTOs
{
    public class VehiclePositionDTO : IMapFrom<VehiclePosition>
    {
        public int Id { get; set; }
        public string RouteNumber { get; set; }
        public DateTime TimeStamp { get; set; }
        public int VehicleNumber { get; set; }
        public double Speed { get; set; }
        public int HeadingDegree { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DoorStatus { get; set; }
        public string LocationSource { get; set; }
        public StopDto? NextStop { get; set; }
        public string Route { get; set; }
        public int Occupants { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<VehiclePosition, VehiclePositionDTO>()
                .ForMember(dest => dest.NextStop, opt => opt.MapFrom(src => src.Stop))
                .ForMember(dest => dest.DoorStatus, opt => opt.MapFrom(src => src.DoorStatus ? "Opened" : "Closed"));
            profile.CreateMap<Stop, StopDto>();
        }
    }
}