using Domain.Compliance.Reports;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Api.Compliance
{
    [ApiController]
    [Route("api/Compliance")]
    [Produces("application/json")]
    public class ComplianceApiController : ControllerBase
    {
        [HttpGet("AccMemo")]
        public async Task<ActionResult<AccMemo>> GetAccMemoAsync(
            [FromServices] IComplianceReportsRepository repository,
            [FromQuery] string facilityId, 
            [FromQuery] int year)
        {
            if (!ApbFacilityId.IsValidAirsNumberFormat(facilityId)) return BadRequest();
            var accMemo = await repository.GetAccMemoAsync(new ApbFacilityId(facilityId), year);
            if (!accMemo.HasValue) return NotFound();
            return Ok(accMemo.Value);
        }
    }
}
