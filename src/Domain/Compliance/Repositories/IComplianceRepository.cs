using Domain.Compliance.Models;

namespace Domain.Compliance.Repositories;

public interface IComplianceRepository
{
    // ACC
    Task<bool> AccReportExistsAsync(ApbFacilityId facilityId, int year);
    Task<AccReport?> GetAccReportAsync(ApbFacilityId facilityId, int year);

    // FCE
    Task<bool> FceReportExistsAsync(ApbFacilityId facilityId, int id);
    Task<FceReport?> GetFceReportAsync(ApbFacilityId facilityId, int id);
}
