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
    public double Latitude { get; set; }

    [Required(ErrorMessage = "Valid Longitude is Required")]
    public double Longitude { get; set; }
    public int WithinDistance { get; set; } = 50;
    public int PageNumber { get; set; } = 1;
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

        var busLocations = _context.VehiclePositions
            .Include(vehicle => vehicle.Stop)
            .Where(vehicle => vehicle.Location.IsWithinDistance(location, request.WithinDistance))
            .AsNoTracking()
            .GroupBy(b => b.VehicleNumber)
            .Select(g => g.First());

        var mappedBusLocations = await busLocations
            .ProjectTo<VehiclePositionDTO>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return mappedBusLocations;
    }
}