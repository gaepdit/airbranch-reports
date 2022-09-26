using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using Domain.Organization.Models;
using Domain.Organization.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Platform.Models;

namespace WebApp.Pages.Compliance.Acc;

public class IndexModel : PageModel
{
    public AccReport? Report { get; set; }
    public OrganizationInfo OrganizationInfo { get; private set; }
    public MemoHeader MemoHeader { get; private set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IComplianceRepository repository,
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

        var getAccReportTask = repository.GetAccReportAsync(airs, id);
        var getOrgTask = orgRepo.GetAsync();

        Report = await getAccReportTask;
        if (Report?.Facility is null) return NotFound();

        MemoHeader = new MemoHeader
        {
            Date = Report.DateComplete,
            From = Report.StaffResponsible.DisplayName,
            Subject = $"Title V Annual Certification for {Report.AccReportingYear}" + Environment.NewLine +
                $"{Report.Facility.Name}, {Report.Facility.FacilityAddress.City}" + Environment.NewLine +
                $"AIRS # {Report.Facility.Id}",
        };

        OrganizationInfo = await getOrgTask;

        return Page();
    }
}
