using Application.Common.Models;
using AutoMapper;
using Journey.Application.Common.Interfaces;
using Journey.Application.Common.Mappings;
using Journey.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.WeatherForecasts.Commands
{
    public class CreateBatchVehiclePositionCommand : IRequest<int>, IMapFrom<VehiclePosition>
    {
        public IEnumerable<VehiclePositionPayload> VehiclePositions { get; set; }
    }

    public class CreateBatchVehiclePositionCommandHandler : IRequestHandler<CreateBatchVehiclePositionCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CreateBatchVehiclePositionCommandHandler> _logger;

        public CreateBatchVehiclePositionCommandHandler(
            IMapper mapper,
            IApplicationDbContext context,
            ILogger<CreateBatchVehiclePositionCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateBatchVehiclePositionCommand request, CancellationToken cancellationToken)
        {
            var positions = _mapper.Map<IEnumerable<VehiclePositionPayload>, IEnumerable<VehiclePosition>>(request.VehiclePositions);

            _context.VehiclePositions.AddRange(positions);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Wrote batch of {positions.Count()} vehicle positions successfully!");
            return positions.Count();
        }
    }
}
