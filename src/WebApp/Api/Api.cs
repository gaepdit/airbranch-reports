using Domain.Facilities.Models;
using Domain.Facilities.Repositories;
using Domain.StackTest.Repositories;

namespace WebApp.Api;

public static class Api
{
    public static void MapApi(this WebApplication webApplication)
    {
        // Add minimal APIs for retrieving facility data.
        webApplication.MapGet("/api/facility/{facilityId}",
            async (IFacilitiesRepository repo, string facilityId) =>
            {
                if (!FacilityId.IsValidFormat(facilityId)) return Results.NotFound();
                var facility = await repo.GetFacilityAsync(new FacilityId(facilityId));
                return facility is not null ? Results.Ok(facility) : Results.NotFound();
            }
        ).WithName("GetFacility").WithOpenApi();

        webApplication.MapGet("/api/facility/{facilityId}/exists",
            async (IFacilitiesRepository repo, string facilityId) =>
                Results.Ok(await repo.FacilityExistsAsync(facilityId))
        ).WithName("FacilityExists").WithOpenApi();

        // Add minimal APIs for retrieving stack test data.
        webApplication.MapGet("/api/facility/{facilityId}/stack-test/{referenceNumber:int}",
            async (IStackTestRepository repo, string facilityId, int referenceNumber) =>
            {
                if (!FacilityId.IsValidFormat(facilityId)) return Results.NotFound();
                var report = await repo.GetStackTestReportAsync(new FacilityId(facilityId), referenceNumber);
                return report is not null ? Results.Ok(report) : Results.NotFound();
            }
        ).WithName("GetStackTestReport").WithOpenApi();
    }
}
