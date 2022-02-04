namespace Domain.ValueObjects;

public record struct ValueWithUnits
(
    string Value,
    string Units,
    string? Preamble = null
)
{
    public string Display() =>
        string.Join(" ", new[] { Preamble, Value, Units }.Where(s => !string.IsNullOrEmpty(s)));
}
