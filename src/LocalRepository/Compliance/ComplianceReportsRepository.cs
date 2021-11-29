using Domain.Compliance.Reports;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using LocalRepository.Data;

namespace LocalRepository.Compliance;

public class ComplianceReportsRepository : IComplianceReportsRepository
{
    public Task<AccMemo?> GetAccMemoAsync(ApbFacilityId facilityId, int year)
    {
        var acc = AccData.GetAccs.SingleOrDefault(e => e.Facility.Id == facilityId && e.AccReportingYear == year);

        if (acc == default) return Task.FromResult(null as AccMemo?);

        var accMemo = new AccMemo
        {
            Acc = acc,
            OrganizationInfo = OrganizationData.GetOrganizationInfo,
        };

        return Task.FromResult(accMemo as AccMemo?);
    }
}
