using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Api.Compliance
{
    [ApiController]
    [Route("api/facility/{facilityId}/acc-report/")]
    [Produces("application/json")]
    public class ComplianceController : ControllerBase
    {
        [HttpGet("{year:int}")]
        public async Task<ActionResult<AccReport>> GetAccReportAsync(
            [FromServices] IComplianceRepository repository,
            [FromRoute] string facilityId,
            [FromRoute] int year)
        {
            if (!ApbFacilityId.IsValidAirsNumberFormat(facilityId)) return BadRequest();
            var accReport = await repository.GetAccReportAsync(new ApbFacilityId(facilityId), year);
            return accReport is null
                ? NotFound()
                : Ok(accReport);
        }
    }
}
