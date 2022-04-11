using MediatR;
using AutoMapper;
using Journey.Application.Common.Interfaces;
using NetTopologySuite.Geometries;
using Microsoft.EntityFrameworkCore;
using Application.VehiclePositions.DTOs;
using System.ComponentModel.DataAnnotations;
using Journey.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using Journey.Application.Common.Mappings;

public class GetNearestBusesQuery : IRequest<PaginatedList<VehiclePositionDTO>>
{
    [Required(ErrorMessage = "Valid Latitude is Required")]
    [Display(Description = "Latitude of location")]
    public double Latitude { get; set; }

    [Required(ErrorMessage = "Valid Longitude is Required")]
    [Display(Description = "Longitude of location")]
    public double Longitude { get; set; }

    [Display(Description = "Radius distance in meters (Default 500)")]
    public int WithinDistance { get; set; } = 500;

    [Display(Description = "Page number (Default 1)")]
    public int PageNumber { get; set; } = 1;

    [Display(Description = "Page size (Default 20)")]
    public int PageSize { get; set; } = 20;

}

public class GetNearestBusesWithPaginationHandler : IRequestHandler<GetNearestBusesQuery, PaginatedList<VehiclePositionDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetNearestBusesWithPaginationHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<VehiclePositionDTO>> Handle(GetNearestBusesQuery request, CancellationToken cancellationToken)
    {
        var location = new Point(request.Longitude, request.Latitude);

        // FIXME Revert to EF query when DISTINCT ON is supported by EF or implement as Stored Procedure
        // Not fan of :(
        var busLocations = _context.VehiclePositions
            .FromSqlRaw(@"
                SELECT 
                    DISTINCT ON (vehicle_number) vehicle_number,
                    id,
                    route_number,
                    direction,
                    operator,
                    time_stamp,
                    speed,
                    heading_degree,
                    latitude,
                    longitude,
                    location,
                    acceleration,
                    door_status,
                    location_source,
                    route,
                    occupants,
                    stop_id
                FROM vehicle_positions
                WHERE ST_DWithin(location, ST_MakePoint({0},{1})::geography, {2}) AND stop_id IS NOT NULL
                ORDER BY vehicle_number, location <-> ST_MakePoint({0},{1})::geography, time_stamp DESC
            ", request.Longitude, request.Latitude, request.WithinDistance)
            .Include(vehicle => vehicle.Stop);

        var mappedBusLocations = await busLocations
            .ProjectTo<VehiclePositionDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return mappedBusLocations;
    }
}