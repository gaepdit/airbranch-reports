using Domain.Facilities.Models;
using Domain.Facilities.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Api.Facility;

[ApiController]
[Route("api/facility/{facilityId}")]
[Produces("application/json")]
public class FacilityController : Controller
{
    [HttpGet]
    public async Task<ActionResult<Domain.Facilities.Models.Facility>> GetAsync(
        [FromServices] IFacilitiesRepository repository,
        [FromRoute] string facilityId)
    {
        if (!ApbFacilityId.IsValidAirsNumberFormat(facilityId)) return BadRequest();
        var facility = await repository.GetFacilityAsync(facilityId);
        return facility is null
            ? NotFound()
            : Ok(facility);
    }

    [HttpGet("exists")]
    public async Task<ActionResult<bool>> ExistsAsync(
        [FromServices] IFacilitiesRepository repository,
        [FromRoute] string facilityId)
    {
        if (!ApbFacilityId.IsValidAirsNumberFormat(facilityId)) return BadRequest();
        return Ok(await repository.FacilityExistsAsync(facilityId));
    }
}
