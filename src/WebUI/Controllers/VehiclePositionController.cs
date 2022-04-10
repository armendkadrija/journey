using Application.VehiclePositions.DTOs;
using Journey.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Journey.WebUI.Controllers;

public class VehiclePositionController : ApiControllerBase
{
    [HttpGet("/buses/near")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(PaginatedList<VehiclePositionDTO>), 200)]
    public async Task<PaginatedList<VehiclePositionDTO>> GetNear([FromQuery] GetNearestBusesQuery query)
    {
        return await Mediator.Send(query);
    }
}
