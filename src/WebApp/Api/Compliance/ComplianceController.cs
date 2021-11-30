using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Api.Compliance
{
    [ApiController]
    [Route("api/facility/{facilityId}/")]
    [Produces("application/json")]
    public class ComplianceController : ControllerBase
    {
        [HttpGet("acc-report")]
        public async Task<ActionResult<AccReport>> GetAccReportAsync(
            [FromServices] IComplianceRepository repository,
            [FromRoute] string facilityId,
            [FromQuery] int year)
        {
            if (!ApbFacilityId.IsValidAirsNumberFormat(facilityId)) return BadRequest();
            var accReport = await repository.GetAccReportAsync(new ApbFacilityId(facilityId), year);
            return !accReport.HasValue
                ? NotFound()
                : Ok(accReport.Value);
        }
    }
}
