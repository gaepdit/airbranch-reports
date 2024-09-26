using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Platform.Models;

namespace WebApp.Pages.Compliance.Acc;

public class IndexModel : PageModel
{
    public AccReport? Report { get; set; }
    public MemoHeader MemoHeader { get; private set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IComplianceRepository repository,
        [FromRoute] string facilityId,
        [FromRoute] int id)
    {
        FacilityId airs;
        try
        {
            airs = new FacilityId(facilityId);
        }
        catch (ArgumentException)
        {
            return NotFound("Facility ID is invalid.");
        }

        Report = await repository.GetAccReportAsync(airs, id);
        if (Report?.Facility is null) return NotFound();

        MemoHeader = new MemoHeader
        {
            Date = Report.DateComplete,
            From = Report.StaffResponsible.DisplayName,
            Subject = $"Title V Annual Certification for {Report.AccReportingYear}" + Environment.NewLine +
                      $"{Report.Facility.Name}, {Report.Facility.FacilityAddress.City}" + Environment.NewLine +
                      $"AIRS # {Report.Facility.Id}",
        };

        return Page();
    }
}
