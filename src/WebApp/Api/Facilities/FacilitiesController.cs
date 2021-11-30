using Domain.Facilities.Models;
using Domain.Facilities.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Api.Facilities
{
    [ApiController]
    [Route("api/facility/{facilityId}")]
    [Produces("application/json")]
    public class FacilitiesController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<Facility>> GetAsync(
            [FromServices] IFacilitiesRepository repository,
            [FromRoute] string facilityId)
        {
            if (!ApbFacilityId.IsValidAirsNumberFormat(facilityId)) return BadRequest();
            var facility = await repository.GetFacilityAsync(facilityId);
            return !facility.HasValue
                ? NotFound()
                : Ok(facility.Value);
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
}
