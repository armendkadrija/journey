using Application.Common.Models;
using Application.VehiclePositions.DTOs;
using AutoMapper;
using Journey.Application.Common.Interfaces;
using Journey.Application.Common.Mappings;
using Journey.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.WeatherForecasts.Commands
{
    public class CreateVehiclePositionCommand : IRequest<int>, IMapFrom<VehiclePosition>
    {
        public VehiclePositionPayload VehiclePosition { get; set; }
    }

    public class CreateVehiclePositionCommandHandler : IRequestHandler<CreateVehiclePositionCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CreateVehiclePositionCommandHandler> _logger;

        public CreateVehiclePositionCommandHandler(
            IMapper mapper,
            IApplicationDbContext context,
            ILogger<CreateVehiclePositionCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateVehiclePositionCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<VehiclePositionPayload, VehiclePosition>(request.VehiclePosition);

            _context.VehiclePositions.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Vehicle Position with Id:{entity.Id} was created successfully");

            return entity.Id;
        }
    }
}
