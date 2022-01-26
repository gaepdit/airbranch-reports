using Domain.StackTest.Models;

namespace Domain.StackTest.Repositories;

public interface IStackTestRepository
{
    Task<bool> StackTestReportExistsAsync(ApbFacilityId facilityId, int referenceNumber);
    Task<DocumentType> GetDocumentTypeAsync(int referenceNumber);
    Task<BaseStackTestReport?> GetStackTestReportAsync(ApbFacilityId facilityId, int referenceNumber);
}
