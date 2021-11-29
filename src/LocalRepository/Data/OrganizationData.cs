using Domain.Organization.Models;

namespace LocalRepository.Data;

internal static class OrganizationData
{
    public static OrganizationInfo GetOrganizationInfo =>
        new() { NameOfDirector = "Richard E. Dunn" };
}
