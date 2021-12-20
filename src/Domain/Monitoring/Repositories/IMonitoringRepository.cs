using Domain.Monitoring.Models;

namespace Domain.Monitoring.Repositories;

public interface IMonitoringRepository
{
    Task<bool> StackTestReportExistsAsync(ApbFacilityId facilityId, int referenceNumber);
    Task<DocumentType> GetDocumentTypeAsync(int referenceNumber);
    Task<BaseStackTestReport?> GetStackTestReportAsync(ApbFacilityId facilityId, int referenceNumber);
}
