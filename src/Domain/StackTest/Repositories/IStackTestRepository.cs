using Domain.StackTest.Models;

namespace Domain.StackTest.Repositories;

public interface IStackTestRepository
{
    Task<BaseStackTestReport?> GetStackTestReportAsync(FacilityId facilityId, int referenceNumber);
}
