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
            State = "GA",
        },
        new()
        {
            Id = new ApbFacilityId("12100021"),
            City = "Btown",
            County = "Bibb",
            Name = "Banana Corp",
            State = "GA",
        },
        new()
        {
            Id = new ApbFacilityId("05100149"),
            City = "Ctown",
            County = "Clay",
            Name = "Cranberry Corp",
            State = "GA",
        },
        new()
        {
            Id = new ApbFacilityId("17900001"),
            City = "Dtown",
            County = "Dade",
            Name = "Date Corp",
            State = "GA",
        },
    };
}
