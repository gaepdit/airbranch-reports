using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Domain.Facilities.Models;

public partial record FacilityId
{
    private readonly string? _id = string.Empty;

    public FacilityId(string id) =>
        _id = IsValidFormat(id)
            ? id.Replace("-", "")
            : throw new ArgumentException($"The value '{id}' is not a valid Facility ID format.");

    // Formats

    public string ShortString => _id ?? throw new InvalidOperationException("Id not initialized.");
    public string FormattedId => $"{ShortString[..3]}-{ShortString[3..8]}";

    [JsonIgnore]
    public string DbFormattedString => $"0413{ShortString}";

    [JsonIgnore]
    public string EpaFacilityIdentifier => $"GA00000013{ShortString}";

    // Operators
    public static implicit operator string(FacilityId id) => id.FormattedId;
    public static implicit operator FacilityId(string id) => new(id);
    public override string ToString() => FormattedId;

    // Format validation
    public static bool IsValidFormat(string id) => FacilityIdRegex().IsMatch(id);

    // FUTURE: Update regex to limit first three digits based on county list.
    // Test at https://regex101.com/r/2uYyHl/4
    // language:regex
    private const string FacilityIdPattern = @"^\d{3}-?\d{5}$";

    [GeneratedRegex(FacilityIdPattern)]
    private static partial Regex FacilityIdRegex();
}
