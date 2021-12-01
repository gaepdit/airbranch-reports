namespace Domain.Personnel;

public record struct Staff
{
    public int Id { get; init; }
    public string GivenName { get; init; }
    public string FamilyName { get; init; }
    public string EmailAddress { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Prefix { get; init; }
    public string? Suffix { get; init; }
    public string? Title { get; init; }

    public string DisplayName =>
        string.Join(" ", new[] { GivenName, FamilyName }.Where(s => !string.IsNullOrEmpty(s)));

    public string SortableFullName =>
        string.Join(", ", new[] { FamilyName, GivenName }.Where(s => !string.IsNullOrEmpty(s)));

}
