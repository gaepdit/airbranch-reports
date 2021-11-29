using Domain.Organization.Models;
using Domain.Organization.Repositories;

namespace Infrastructure.Organization;

public class OrganizationRepository : IOrganizationRepository
{
    public Task<OrganizationInfo> GetAsync()
    {
        throw new NotImplementedException();
    }
}
