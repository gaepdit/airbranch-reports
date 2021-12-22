using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using static LocalRepository.Data.AccData;

namespace LocalRepository.Compliance;

public class ComplianceRepository : IComplianceRepository
{
    public Task<bool> AccReportExistsAsync(ApbFacilityId facilityId, int year) =>
        Task.FromResult(GetAccReports.Any(e => e.Facility?.Id == facilityId && e.AccReportingYear == year));

    public async Task<AccReport?> GetAccReportAsync(ApbFacilityId facilityId, int year) =>
        await AccReportExistsAsync(facilityId, year).ConfigureAwait(false)
        ? GetAccReports.SingleOrDefault(e => e.Facility?.Id == facilityId && e.AccReportingYear == year)
        : null;
}
