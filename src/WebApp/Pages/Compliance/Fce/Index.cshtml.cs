using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Compliance.Fce;

public class IndexModel : PageModel
{
    public FceReport? Report { get; set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IComplianceRepository complianceRepo,
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

        Report = await complianceRepo.GetFceReportAsync(airs, id);
        if (Report?.Facility is null) return NotFound();
        if (Report.Facility.HeaderData is null) return NotFound();

        return Page();
    }
}
