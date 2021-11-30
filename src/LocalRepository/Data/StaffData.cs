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
            FirstName = "Adélie",
            LastName = "Penguin",
        },
        new()
        {
            Id = 2,
            EmailAddress = "two@example.net",
            FirstName = "Bactrian",
            LastName = "Camel",
        },
        new()
        {
            Id = 3,
            EmailAddress = "three@example.net",
            FirstName = "Clouded",
            LastName = "Leopard",
        },
    };
}
