using System.Text.RegularExpressions;

namespace Domain.Facilities.Models;

public record struct ApbFacilityId : IEquatable<ApbFacilityId>
{
    private readonly string _id;

    public ApbFacilityId(string id)
    {
        _id = IsValidAirsNumberFormat(id)
            ? GetNormalizedAirsNumber(id)
            : throw new ArgumentException($"{id} is not a valid AIRS number.");
    }

    public static implicit operator ApbFacilityId(string id) => new(id);

    public override string ToString() => _id;
    public string ShortString => _id;
    public string FormattedString => $"{_id[..3]}-{_id[3..8]}";
    public string DbFormattedString => $"0413{ShortString}";
    public string EpaFacilityIdentifier => $"GA00000013{ShortString}";

    private const string AirsNumberPattern = @"^(04-?13-?)?\d{3}-?\d{5}$";

    public static bool IsValidAirsNumberFormat(string id) =>
        !string.IsNullOrWhiteSpace(id) && Regex.IsMatch(id, AirsNumberPattern);

    private static string GetNormalizedAirsNumber(string id)
    {
        id = id.Replace("-", "");
        return id.Length == 12 ? id.Remove(0, 4) : id;
    }
}
