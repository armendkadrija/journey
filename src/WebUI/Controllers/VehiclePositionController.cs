using Application.VehiclePositions.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Journey.WebUI.Controllers;

public class VehiclePositionController : ApiControllerBase
{
    [HttpGet("/buses/near")]
    public async Task<IEnumerable<VehiclePositionDTO>> GetNear([FromQuery] GetNearestBusesQuery query)
    {
        return await Mediator.Send(query);
    }
}
