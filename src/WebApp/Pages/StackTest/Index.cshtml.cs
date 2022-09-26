using Domain.Facilities.Models;
using Domain.Organization.Models;
using Domain.Organization.Repositories;
using Domain.StackTest.Models;
using Domain.StackTest.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Platform.Models;

namespace WebApp.Pages.StackTest;

public class IndexModel : PageModel
{
    public BaseStackTestReport? Report { get; private set; }
    public OrganizationInfo OrganizationInfo { get; private set; }
    public MemoHeader MemoHeader { get; private set; }
    public bool ShowConfidentialWarning { get; private set; }

    public async Task<ActionResult> OnGetAsync(
        [FromServices] IStackTestRepository repository,
        [FromServices] IOrganizationRepository orgRepo,
        [FromRoute] string facilityId,
        [FromRoute] int referenceNumber,
        [FromQuery] bool includeConfidentialInfo = false)
    {
        if (includeConfidentialInfo && User.Identity is not { IsAuthenticated: true })
            return Challenge();

        ApbFacilityId airs;
        try
        {
            airs = new ApbFacilityId(facilityId);
        }
        catch (ArgumentException)
        {
            return NotFound("Facility ID is invalid.");
        }

        var getReportTask = repository.GetStackTestReportAsync(airs, referenceNumber);
        var getOrgTask = orgRepo.GetAsync();

        var report = await getReportTask;
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
        OrganizationInfo = await getOrgTask;

        return Page();
    }
}
