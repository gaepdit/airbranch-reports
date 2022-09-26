using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using Domain.Organization.Models;
using Domain.Organization.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Compliance.Fce;

public class IndexModel : PageModel
{
    public FceReport? Report { get; set; }
    public OrganizationInfo OrganizationInfo { get; set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IComplianceRepository complianceRepo,
        [FromServices] IOrganizationRepository orgRepo,
        [FromRoute] string facilityId,
        [FromRoute] int id)
    {
        ApbFacilityId airs;
        try
        {
            airs = new ApbFacilityId(facilityId);
        }
        catch (ArgumentException)
        {
            return NotFound("Facility ID is invalid.");
        }

        var getReportTask = complianceRepo.GetFceReportAsync(airs, id);
        var getOrgTask = orgRepo.GetAsync();

        Report = await getReportTask;
        if (Report?.Facility is null) return NotFound();
        if (Report.Facility.HeaderData is null) return NotFound();

        OrganizationInfo = await getOrgTask;

        return Page();
    }
}
