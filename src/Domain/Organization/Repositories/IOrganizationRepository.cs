using Domain.Organization.Models;

namespace Domain.Organization.Repositories;

public interface IOrganizationRepository
{
    Task<OrganizationInfo> GetAsync();
}
