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
            Name = new PersonName
            {
                GivenName = "Adélie",
                FamilyName = "Penguin",
            },
        },
        new()
        {
            Id = 2,
            EmailAddress = "two@example.net",
            Name = new PersonName
            {
                GivenName = "Bactrian",
                FamilyName = "Camel",
            },
        },
        new()
        {
            Id = 3,
            EmailAddress = "three@example.net",
            Name = new PersonName
            {
                GivenName = "Clouded",
                FamilyName = "Leopard",
                Suffix = "Jr."
            }
        },
        new()
        {
            Id = 4,
            EmailAddress = "four@example.net",
            Name = new PersonName("Dugong", "Sirenia", "Ms.", null)
        },
        new()
        {
            Id = 5,
            EmailAddress = "five@example.net",
            Name = new PersonName
            {
                GivenName = "Elephant",
                FamilyName = "Seal",
            }
        },
        new()
        {
            Id = 6,
            EmailAddress = "six@example.net",
            Name = new PersonName
            {
                GivenName = "Fennec",
                FamilyName = "Fox",
            }
        },
    };
}
