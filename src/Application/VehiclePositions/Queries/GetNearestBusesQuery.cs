using MediatR;
using AutoMapper;
using Journey.Application.Common.Interfaces;
using NetTopologySuite.Geometries;
using Microsoft.EntityFrameworkCore;
using Application.VehiclePositions.DTOs;
using Journey.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class GetNearestBusesQuery : IRequest<IEnumerable<VehiclePositionDTO>>
{
    [Required(ErrorMessage = "Valid Latitude is Required")]
    public double Latitude { get; set; }

    [Required(ErrorMessage = "Valid Longitude is Required")]
    public double Longitude { get; set; }
    public int WithinDistance { get; set; } = 50;

}

public class GetNearestBusesWithPaginationHandler : IRequestHandler<GetNearestBusesQuery, IEnumerable<VehiclePositionDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetNearestBusesWithPaginationHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VehiclePositionDTO>> Handle(GetNearestBusesQuery request, CancellationToken cancellationToken)
    {
        var location = new Point(request.Longitude, request.Latitude);

        var result = await _context.VehiclePositions
            .Include(vehicle => vehicle.Stop)
            .Where(vehicle => vehicle.Location.IsWithinDistance(location, request.WithinDistance))
            .OrderBy(vehicle => vehicle.Location.Distance(location))
            .Take(30)
            .ToListAsync();

        return _mapper.Map<IEnumerable<VehiclePosition>, IEnumerable<VehiclePositionDTO>>(result);
    }
}