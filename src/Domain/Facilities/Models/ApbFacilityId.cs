using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Domain.Facilities.Models;

public record class ApbFacilityId : IEquatable<ApbFacilityId>
{
    private readonly string _id;

    public ApbFacilityId(string id)
    {
        _id = IsValidAirsNumberFormat(id)
            ? GetNormalizedAirsNumber(id)
            : throw new ArgumentException($"{id} is not a valid AIRS number.");
    }

    public static implicit operator ApbFacilityId(string id) => new(id);

    public override string ToString() => FacilityId;
    public string FacilityId => $"{_id[..3]}-{_id[3..8]}";

    [JsonIgnore]
    public string ShortString => _id;
    [JsonIgnore]
    public string DbFormattedString => $"0413{ShortString}";
    [JsonIgnore]
    public string EpaFacilityIdentifier => $"GA00000013{ShortString}";

    private const string AirsNumberPattern = @"^(04-?13-?)?\d{3}-?\d{5}$";

    // Static methods

    public static bool IsValidAirsNumberFormat(string id) =>
        !string.IsNullOrWhiteSpace(id) && Regex.IsMatch(id, AirsNumberPattern);

    private static string GetNormalizedAirsNumber(string id)
    {
        id = id.Replace("-", "");
        return id.Length == 12 ? id.Remove(0, 4) : id;
    }
}
