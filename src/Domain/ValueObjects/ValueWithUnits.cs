namespace Domain.ValueObjects;

public record struct ValueWithUnits
(
    string Value,
    string Units
)
{
    public override string ToString() =>
        string.Join(" ", new[] { Value, Units }.Where(s => !string.IsNullOrEmpty(s)));
}
