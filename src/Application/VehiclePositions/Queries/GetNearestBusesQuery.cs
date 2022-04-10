using MediatR;
using AutoMapper;
using Journey.Application.Common.Interfaces;

public class GetNearestBusesQuery : IRequest<int>
{
}

public class GetNearestBusesWithPaginationHandler : IRequestHandler<GetNearestBusesQuery, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetNearestBusesWithPaginationHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(GetNearestBusesQuery request, CancellationToken cancellationToken)
    {
        var firstElement = await _context.VehiclePositions.FindAsync(3);

        return _context.VehiclePositions.Count();
    }
}