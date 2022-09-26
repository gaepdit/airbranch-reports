using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Domain.Facilities.Models;

public record ApbFacilityId
{
    // Test at https://regex101.com/r/2uYyHl
    private const string AirsNumberPattern = @"^(04-?13-?)?\d{3}-?\d{5}$";

    public ApbFacilityId(string id) =>
        ShortString = IsValidAirsNumberFormat(id)
            ? GetNormalizedAirsNumber(id)
            : throw new ArgumentException($"{id} is not a valid AIRS number.");

    public string FacilityId => $"{ShortString[..3]}-{ShortString[3..8]}";

    [JsonIgnore]
    public string ShortString { get; }

    [JsonIgnore]
    public string DbFormattedString => $"0413{ShortString}";

    [JsonIgnore]
    public string EpaFacilityIdentifier => $"GA00000013{ShortString}";

    public static implicit operator ApbFacilityId(string id) => new(id);

    public override string ToString() => FacilityId;

    // Static methods

    public static bool IsValidAirsNumberFormat(string id) => Regex.IsMatch(id, AirsNumberPattern);

    private static string GetNormalizedAirsNumber(string id)
    {
        var newId = id.Replace("-", "");
        return newId.Length == 12 ? newId.Remove(0, 4) : newId;
    }
}
