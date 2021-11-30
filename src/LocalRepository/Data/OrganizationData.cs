using Domain.Organization.Models;

namespace LocalRepository.Data;

public static class OrganizationData
{
    public static OrganizationInfo GetOrganizationInfo =>
        new() { NameOfDirector = "Richard E. Dunn" };
}
