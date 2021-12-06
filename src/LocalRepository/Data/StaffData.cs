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
        new()
        {
            Id = 4,
            EmailAddress = "four@example.net",
            GivenName = "Dugong",
            FamilyName = "Sirenia",
        },
        new()
        {
            Id = 5,
            EmailAddress = "five@example.net",
            GivenName = "Elephant",
            FamilyName = "Seal",
        },
        new()
        {
            Id = 6,
            EmailAddress = "six@example.net",
            GivenName = "Fennec",
            FamilyName = "Fox",
        },
    };
}
