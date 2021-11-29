using Domain.Compliance.Reports;
using Domain.Facilities.Models;

namespace Domain.Compliance.Repositories;

public interface IComplianceReportsRepository
{
    Task<AccMemo?> GetAccMemoAsync(ApbFacilityId id, int year);
}
