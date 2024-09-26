using Domain.Compliance.Models;

namespace Domain.Compliance.Repositories;

public interface IComplianceRepository
{
    // ACC
    Task<AccReport?> GetAccReportAsync(FacilityId facilityId, int id);

    // FCE
    Task<FceReport?> GetFceReportAsync(FacilityId facilityId, int id);
}
