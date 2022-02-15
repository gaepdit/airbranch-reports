using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Api.Compliance;

[ApiController]
[Route("api/facility/{facilityId}")]
[Produces("application/json")]
public class ComplianceController : ControllerBase
{
    [HttpGet("acc-report/{year:int}")]
    public async Task<ActionResult<AccReport>> GetAccReportAsync(
        [FromServices] IComplianceRepository repository,
        [FromRoute] string facilityId,
        [FromRoute] int year)
    {
        if (!ApbFacilityId.IsValidAirsNumberFormat(facilityId)) return BadRequest();
        var report = await repository.GetAccReportAsync(new ApbFacilityId(facilityId), year);
        return report is null
            ? NotFound()
            : Ok(report);
    }

    [HttpGet("fce/{id:int}")]
    public async Task<ActionResult<AccReport>> GetFceReportAsync(
        [FromServices] IComplianceRepository repository,
        [FromRoute] string facilityId,
        [FromRoute] int id)
    {
        if (!ApbFacilityId.IsValidAirsNumberFormat(facilityId)) return BadRequest();
        var report = await repository.GetFceReportAsync(new ApbFacilityId(facilityId), id);
        return report is null
            ? NotFound()
            : Ok(report);
    }
}
