using Domain.Facilities.Models;
using Domain.Organization.Models;
using Domain.StackTest.Models;
using Domain.StackTest.Repositories;
using Domain.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Platform;

namespace WebApp.Pages.StackTest;

public class IndexModel : PageModel
{
    public BaseStackTestReport? Report { get; private set; }
    public MemoHeader MemoHeader { get; private set; }
    public OrganizationInfo OrganizationInfo { get; private set; } = default!;
    public bool ShowConfidentialWarning { get; private set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IStackTestRepository repository,
        [FromRoute] string facilityId,
        [FromRoute] int referenceNumber,
        [FromQuery] bool includeConfidentialInfo = false)
    {
        if (includeConfidentialInfo)
        {
            if (User.Identity is not { IsAuthenticated: true }) return Challenge();
            if (User.Identity.Name is null || !User.Identity.Name.IsValidEmailDomain()) return Forbid();
        }

        ApbFacilityId airs;
        try
        {
            airs = new ApbFacilityId(facilityId);
        }
        catch (ArgumentException)
        {
            return NotFound("Facility ID is invalid.");
        }

        var report = await repository.GetStackTestReportAsync(airs, referenceNumber);
        if (report?.Facility is null) return NotFound();

        Report = includeConfidentialInfo ? report : report.RedactedStackTestReport();
        MemoHeader = new MemoHeader
        {
            To = Report.ComplianceManager.DisplayName,
            From = Report.ReviewedByStaff.DisplayName,
            Through = Report.TestingUnitManager.DisplayName,
            Subject = Report.ReportTypeSubject.ToUpperInvariant(),
        };
        ShowConfidentialWarning = includeConfidentialInfo && Report.ConfidentialParameters.Any();
        OrganizationInfo = AppSettings.OrganizationInfo with { NameOfDirector = report.EpdDirector };
        return Page();
    }
}
