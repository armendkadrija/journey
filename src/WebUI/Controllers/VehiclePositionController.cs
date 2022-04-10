using Microsoft.AspNetCore.Mvc;

namespace Journey.WebUI.Controllers;

public class VehiclePositionController : ApiControllerBase
{
    [HttpGet("/buses/near")]
    public async Task<int> GetNear()
    {
        return await Mediator.Send(new GetNearestBusesQuery());
    }
}
