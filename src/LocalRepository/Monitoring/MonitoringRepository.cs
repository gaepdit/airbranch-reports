using Domain.Monitoring.Models;
using Domain.Monitoring.Repositories;
using static LocalRepository.Data.MonitoringData;

namespace LocalRepository.Monitoring;

public class MonitoringRepository : IMonitoringRepository
{
    public async Task<StackTestReport?> GetStackTestReportAsync(ApbFacilityId facilityId, int referenceNumber)
    {
        if (!await StackTestReportExistsAsync(facilityId, referenceNumber)) return null;

        var result = GetStackTestReports.Single(e => e.ReferenceNumber == referenceNumber);
        result.ParseConfidentialParameters();
        return result;
    }

    public Task<bool> StackTestReportExistsAsync(ApbFacilityId facilityId, int referenceNumber) =>
        Task.FromResult(GetStackTestReports.Any(e => 
        e.ReferenceNumber == referenceNumber && 
        e.Facility.Id == facilityId &&
        e.DocumentType != DocumentType.Unassigned));

    public Task<DocumentType> GetDocumentTypeAsync(int referenceNumber) =>
        Task.FromResult(GetStackTestReports.Single(e => e.ReferenceNumber == referenceNumber).DocumentType);
}
