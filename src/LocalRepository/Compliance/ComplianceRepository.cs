using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using static LocalRepository.Data.AccData;
using static LocalRepository.Data.FceData;

namespace LocalRepository.Compliance;

public class ComplianceRepository : IComplianceRepository
{
    // ACC
    public Task<AccReport?> GetAccReportAsync(ApbFacilityId facilityId, int id) =>
        AccReports.Any(e => e.Facility?.Id == facilityId && e.Id == id)
            ? Task.FromResult(AccReports.SingleOrDefault(e => e.Facility?.Id == facilityId && e.Id == id))
            : Task.FromResult(null as AccReport);

    // FCE
    public Task<FceReport?> GetFceReportAsync(ApbFacilityId facilityId, int id) =>
        FceReports.Any(e => e.Facility?.Id == facilityId && e.Id == id)
            ? Task.FromResult(FceReports.SingleOrDefault(e => e.Facility?.Id == facilityId && e.Id == id))
            : Task.FromResult(null as FceReport);
}
