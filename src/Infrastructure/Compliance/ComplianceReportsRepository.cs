using Domain.Compliance.Reports;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;

namespace Infrastructure.Compliance;

public class ComplianceReportsRepository : IComplianceReportsRepository
{
    public Task<AccMemo?> GetAccMemoAsync(ApbFacilityId id, int year)
    {
        throw new NotImplementedException();
    }
}
