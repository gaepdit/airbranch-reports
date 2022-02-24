using Domain.StackTest.Models;

namespace Domain.StackTest.Repositories;

public interface IStackTestRepository
{
    Task<BaseStackTestReport?> GetStackTestReportAsync(ApbFacilityId facilityId, int referenceNumber);
}
