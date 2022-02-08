using Domain.Organization.Models;

namespace LocalRepository.Data;

public static class OrganizationData
{
    public static OrganizationInfo Organization =>
        new()
        {
            NameOfDirector = "Richard E. Dunn",
            Org = "Air Protection Branch",
            OrgAddress = new Address
            {
                Street = "4244 International Parkway, Suite 120",
                Street2 = null,
                City = "Atlanta",
                State = "Georgia",
                PostalCode = "30354",
            },
            OrgPhoneNumber = "404-363-7000",
        };
}
