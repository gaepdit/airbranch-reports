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
    public Task<bool> FceReportExistsAsync(ApbFacilityId facilityId, int id) =>
        Task.FromResult(GetFceReports.Any(e => e.Facility?.Id == facilityId && e.Id == id));

    public async Task<FceReport?> GetFceReportAsync(ApbFacilityId facilityId, int id) =>
        await FceReportExistsAsync(facilityId, id).ConfigureAwait(false)
        ? GetFceReports.SingleOrDefault(e => e.Facility?.Id == facilityId && e.Id == id)
        : null;
}
