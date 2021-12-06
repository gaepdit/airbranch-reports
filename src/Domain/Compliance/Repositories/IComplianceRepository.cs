using Domain.Compliance.Models;

namespace Domain.Compliance.Repositories;

public interface IComplianceRepository
{
    Task<bool> AccReportExistsAsync(ApbFacilityId facilityId, int year);
    Task<AccReport?> GetAccReportAsync(ApbFacilityId facilityId, int year);
}
