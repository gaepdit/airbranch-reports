using Domain.Facilities.Models;
using Domain.Monitoring.Models;
using Domain.Monitoring.Repositories;

namespace Infrastructure.Monitoring;

public class MonitoringRepository : IMonitoringRepository
{
    public Task<BaseStackTestReport?> GetStackTestReportAsync(ApbFacilityId facilityId, int referenceNumber)
    {
        throw new NotImplementedException();
    }

    public Task<bool> StackTestReportExistsAsync(ApbFacilityId facilityId, int referenceNumber)
    {
        throw new NotImplementedException();
    }
}
