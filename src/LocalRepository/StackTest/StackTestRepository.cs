using Domain.StackTest.Models;
using Domain.StackTest.Repositories;
using static LocalRepository.Data.StackTestData;

namespace LocalRepository.StackTest;

public class StackTestRepository : IStackTestRepository
{
    public Task<bool> StackTestReportExistsAsync(ApbFacilityId facilityId, int referenceNumber) =>
        Task.FromResult(GetStackTestReports.Any(e =>
        e.ReferenceNumber == referenceNumber &&
        e.Facility?.Id == facilityId &&
        e.DocumentType != DocumentType.Unassigned));

    public Task<DocumentType> GetDocumentTypeAsync(int referenceNumber) =>
        Task.FromResult(GetStackTestReports.Single(e => e.ReferenceNumber == referenceNumber).DocumentType);

    public async Task<BaseStackTestReport?> GetStackTestReportAsync(ApbFacilityId facilityId, int referenceNumber)
    {
        if (!await StackTestReportExistsAsync(facilityId, referenceNumber)) return null;

        var result = GetStackTestReports.Single(e => e.ReferenceNumber == referenceNumber);
        result.ParseConfidentialParameters();
        return result;
    }
}
