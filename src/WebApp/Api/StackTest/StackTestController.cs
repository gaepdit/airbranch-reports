using Domain.Facilities.Models;
using Domain.StackTest.Models;
using Domain.StackTest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Api.StackTest
{
    [ApiController]
    [Route("api/facility/{facilityId}/stack-test")]
    [Produces("application/json")]
    public class StackTestController : ControllerBase
    {
        [HttpGet("{referenceNumber:int}")]
        public async Task<ActionResult<BaseStackTestReport>> GetAsync(
            [FromServices] IStackTestRepository repository,
            [FromRoute] string facilityId,
            [FromRoute] int referenceNumber,
            [FromQuery] bool includeConfidentialInfo = false)
        {
            if (!ApbFacilityId.IsValidAirsNumberFormat(facilityId)) return BadRequest();
            var stackTestReport = await repository.GetStackTestReportAsync(facilityId, referenceNumber);
            if (stackTestReport is null) return NotFound();

            if (includeConfidentialInfo && (User.Identity == null || !User.Identity.IsAuthenticated))
                return Challenge();

            return includeConfidentialInfo ? Ok(stackTestReport) : Ok(stackTestReport.RedactedStackTestReport());
        }
    }
}
