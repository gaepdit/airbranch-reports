using Domain.Personnel;

namespace LocalRepository.Data;

public static class StaffData
{
    public static IEnumerable<Staff> GetStaff => new List<Staff>
    {
        new()
        {
            Id = 1,
            EmailAddress = "one@example.net",
            GivenName = "Adélie",
            FamilyName = "Penguin",
        },
        new()
        {
            Id = 2,
            EmailAddress = "two@example.net",
            GivenName = "Bactrian",
            FamilyName = "Camel",
        },
        new()
        {
            Id = 3,
            EmailAddress = "three@example.net",
            GivenName = "Clouded",
            FamilyName = "Leopard",
        },
    };
}
