namespace LocalRepository.Data;

public static class FacilityData
{
    public static IEnumerable<Facility> GetFacilities => new List<Facility>
    {
        new()
        {
            Id = new ApbFacilityId("00100001"),
            City = "Atown",
            County = "Appling",
            Name = "Apple Corp",
            State = "Georgia",
        },
        new()
        {
            Id = new ApbFacilityId("00200002"),
            City = "Btown",
            County = "Bibb",
            Name = "Banana Corp",
            State = "Georgia",
        },
        new()
        {
            Id = new ApbFacilityId("00300003"),
            City = "Ctown",
            County = "Clay",
            Name = "Cranberry Corp",
            State = "Georgia",
        },
    };
}
