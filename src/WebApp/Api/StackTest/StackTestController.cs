using Domain.Facilities.Models;
using Domain.Monitoring.Models;
using Domain.Monitoring.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Api.StackTest
{
    [ApiController]
    [Route("api/facility/{facilityId}/stack-test")]
    [Produces("application/json")]
    public class StackTestController : ControllerBase
    {
        [HttpGet("{referenceNumber:int}")]
        public async Task<ActionResult<StackTestReport>> GetAsync(
            [FromServices] IMonitoringRepository repository,
            [FromRoute] string facilityId,
            [FromRoute] int referenceNumber,
            [FromQuery] bool includeConfidentialInfo = false)
        {
            if (!ApbFacilityId.IsValidAirsNumberFormat(facilityId)) return BadRequest();
            var stackTestReport = await repository.GetStackTestReportAsync(facilityId, referenceNumber);
            if (stackTestReport is null) return NotFound();

            if (includeConfidentialInfo)
            {
                // TODO: check authentication
                return Ok(stackTestReport);
            }

            return Ok(stackTestReport.RedactedStackTestReport());
        }

        [HttpGet("exists")]
        public async Task<ActionResult<bool>> ExistsAsync(
            [FromServices] IMonitoringRepository repository,
            [FromRoute] string facilityId,
            [FromRoute] int referenceNumber) =>
            Ok(await repository.StackTestReportExistsAsync(facilityId, referenceNumber));
    }
}
