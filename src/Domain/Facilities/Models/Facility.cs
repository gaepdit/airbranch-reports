namespace Domain.Facilities.Models;

public record struct Facility
{
    public ApbFacilityId Id { get; init; }
    public string Name { get; init; }
    public string City { get; init; }
    public string County { get; init; }
}
