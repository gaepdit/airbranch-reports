namespace Domain.Personnel;

public record struct Staff
{
    public int Id { get; init; } 
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string EmailAddress { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Prefix { get; init; }
    public string? Suffix { get; init; }
    public string? Title { get; init; }
}
