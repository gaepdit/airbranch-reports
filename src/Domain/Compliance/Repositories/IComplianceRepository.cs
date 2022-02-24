using Domain.Compliance.Models;

namespace Domain.Compliance.Repositories;

public interface IComplianceRepository
{
    // ACC
    Task<AccReport?> GetAccReportAsync(ApbFacilityId facilityId, int year);

    // FCE
    Task<FceReport?> GetFceReportAsync(ApbFacilityId facilityId, int id);
}
