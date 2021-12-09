using Domain.Monitoring.Models;

namespace Domain.Monitoring.Repositories;

public interface IMonitoringRepository
{
    Task<bool> StackTestReportExistsAsync(ApbFacilityId facilityId, int referenceNumber);
    Task<StackTestReport?> GetStackTestReportAsync(ApbFacilityId facilityId, int referenceNumber);
}
