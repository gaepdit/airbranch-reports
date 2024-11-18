using Domain.StackTest.Models;
using Domain.StackTest.Repositories;
using static LocalRepository.Data.StackTestData;

namespace LocalRepository.StackTest;

public class StackTestRepository : IStackTestRepository
{
    private static bool StackTestReportExists(FacilityId facilityId, int referenceNumber) =>
        StackTestReports.Any(e =>
            e.ReferenceNumber == referenceNumber &&
            e.Facility?.Id == facilityId &&
            e.DocumentType != DocumentType.Unassigned);

    public Task<BaseStackTestReport?> GetStackTestReportAsync(FacilityId facilityId, int referenceNumber)
    {
        if (!StackTestReportExists(facilityId, referenceNumber))
            return Task.FromResult(null as BaseStackTestReport);

        var result = StackTestReports.SingleOrDefault(e => e.ReferenceNumber == referenceNumber);
        result?.ParseConfidentialParameters();
        return Task.FromResult(result);
    }
}
