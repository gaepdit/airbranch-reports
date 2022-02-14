using Domain.Organization.Models;
using Domain.Organization.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Api.Organization;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrganizationController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<OrganizationInfo>> GetAsync(
        [FromServices] IOrganizationRepository repository) =>
        Ok(await repository.GetAsync());
}
