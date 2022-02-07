using Domain.Organization.Models;
using Domain.Organization.Repositories;

namespace LocalRepository.Organization;

public class OrganizationRepository : IOrganizationRepository
{
    public Task<OrganizationInfo> GetAsync() =>
        Task.FromResult(OrganizationData.Organization);
}
