using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using Domain.Organization.Models;
using Domain.Organization.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Facility.Compliance;

public class ACC_ReportModel : PageModel
{
    public AccReport AccReport { get; set; }
    public OrganizationInfo OrganizationInfo { get; set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IComplianceRepository repository,
        [FromServices] IOrganizationRepository orgRepo,
        [FromQuery] string facilityId,
        [FromQuery] int year)
    {
        if (!ApbFacilityId.IsValidAirsNumberFormat(facilityId)) return NotFound($"Facility ID {facilityId} is invalid.");

        var report = await repository.GetAccReportAsync(new ApbFacilityId(facilityId), year);
        if (report == null) return NotFound();

        AccReport = report.Value;
        OrganizationInfo = await orgRepo.GetAsync();

        return Page();
    }
}
