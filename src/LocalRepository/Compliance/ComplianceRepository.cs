using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using static LocalRepository.Data.AccData;
using static LocalRepository.Data.FceData;

namespace LocalRepository.Compliance;

public class ComplianceRepository : IComplianceRepository
{
    // ACC
    public Task<bool> AccReportExistsAsync(ApbFacilityId facilityId, int year) =>
        Task.FromResult(GetAccReports.Any(e => e.Facility?.Id == facilityId && e.AccReportingYear == year));

    public async Task<AccReport?> GetAccReportAsync(ApbFacilityId facilityId, int year) =>
        await AccReportExistsAsync(facilityId, year).ConfigureAwait(false)
        ? GetAccReports.SingleOrDefault(e => e.Facility?.Id == facilityId && e.AccReportingYear == year)
        : null;

    // FCE
    public Task<bool> FceReportExistsAsync(ApbFacilityId facilityId, int year) =>
        Task.FromResult(GetFceReports.Any(e => e.Facility?.Id == facilityId && e.FceYear == year));

    public async Task<FceReport?> GetFceReportAsync(ApbFacilityId facilityId, int year) =>
        await FceReportExistsAsync(facilityId, year).ConfigureAwait(false)
        ? GetFceReports.SingleOrDefault(e => e.Facility?.Id == facilityId && e.FceYear == year)
        : null;
}
