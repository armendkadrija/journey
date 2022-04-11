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

    [Display(Description = "Distance radius in meters (Default 500)")]
    public int WithinDistance { get; set; } = 500;

    [Display(Description = "Time from last known location in seconds (Default 5)")]
    public int WithinSeconds { get; set; } = 5;

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
        var secondsAgo = DateTime.Now.AddSeconds(request.WithinSeconds * -1);

        var busLocations = _context.VehiclePositions
            .Include(vehicle => vehicle.Stop)
            .Where(vehicle =>
                vehicle.Location.IsWithinDistance(location, request.WithinDistance) &&
                vehicle.TimeStamp >= secondsAgo
            )
            .OrderBy(vehicle => vehicle.Location.Distance(location))
            .OrderByDescending(vehicle => vehicle.TimeStamp);

        var mappedBusLocations = await busLocations
            .ProjectTo<VehiclePositionDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return mappedBusLocations;
    }
}