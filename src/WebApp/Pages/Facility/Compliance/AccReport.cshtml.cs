using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using Domain.Organization.Models;
using Domain.Organization.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Platform.Models;

namespace WebApp.Pages.Facility.Compliance;

public class AccReportModel : PageModel
{
    public AccReport AccReport { get; set; }
    public OrganizationInfo OrganizationInfo { get; set; }
    public MemoHeader MemoHeader { get; set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IComplianceRepository repository,
        [FromServices] IOrganizationRepository orgRepo,
        [FromRoute] string facilityId,
        [FromRoute] int year)
    {
        if (!ApbFacilityId.IsValidAirsNumberFormat(facilityId)) return NotFound($"Facility ID {facilityId} is invalid.");

        var report = await repository.GetAccReportAsync(new ApbFacilityId(facilityId), year);
        if (report == null) return NotFound();

        AccReport = report.Value;
        OrganizationInfo = await orgRepo.GetAsync();

        MemoHeader = new MemoHeader
        {
            Date = AccReport.DateAcknowledgmentLetterSent,
            From = AccReport.StaffResponsible.DisplayName,
            Subject = $"Title V Annual Certification for {AccReport.AccReportingYear}" + Environment.NewLine +
                $"{AccReport.Facility.Name}, {AccReport.Facility.City}" + Environment.NewLine +
                $"AIRS # {AccReport.Facility.Id}",
        };

        return Page();
    }
}
