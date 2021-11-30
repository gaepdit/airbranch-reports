using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;

namespace Infrastructure.Compliance;

public class ComplianceRepository : IComplianceRepository
{
    public Task<bool> AccReportExistsAsync(ApbFacilityId facilityId, int year)
    {
        throw new NotImplementedException();
    }

    public Task<AccReport?> GetAccReportAsync(ApbFacilityId id, int year)
    {
        throw new NotImplementedException();
    }
}
