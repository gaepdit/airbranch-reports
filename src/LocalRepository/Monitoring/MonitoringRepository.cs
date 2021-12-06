using Domain.Monitoring.Models;
using Domain.Monitoring.Repositories;
using static LocalRepository.Data.StackTests.StackTestData;

namespace LocalRepository.Monitoring;

public class MonitoringRepository : IMonitoringRepository
{
    public async Task<StackTestReport?> GetStackTestReportAsync(ApbFacilityId facilityId, int referenceNumber) =>
        await StackTestReportExistsAsync(facilityId, referenceNumber)
        ? GetStackTestReports.SingleOrDefault(e =>
            e.ReferenceNumber == referenceNumber &&
            e.Facility.Id == facilityId)
        : null;

    public Task<bool> StackTestReportExistsAsync(ApbFacilityId facilityId, int referenceNumber) =>
        Task.FromResult(GetStackTestReports.Any(e =>
            e.ReferenceNumber == referenceNumber &&
            e.Facility.Id == facilityId));
}
