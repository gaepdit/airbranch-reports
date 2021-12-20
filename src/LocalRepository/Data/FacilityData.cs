namespace LocalRepository.Data;

public static class FacilityData
{
    public static IEnumerable<Facility> GetFacilities => new List<Facility>
    {
        new()
        {
            Id = new ApbFacilityId("00100001"),
            City = "Atlantis",
            County = "Appling",
            Name = "Apple Corp",
            State = "GA",
        },
        new()
        {
            Id = new ApbFacilityId("12100021"),
            City = "Bedford Falls",
            County = "Bibb",
            Name = "Banana Corp",
            State = "GA",
        },
        new()
        {
            Id = new ApbFacilityId("05100149"),
            City = "Coruscant",
            County = "Clay",
            Name = "Cranberry Corp",
            State = "GA",
        },
        new()
        {
            Id = new ApbFacilityId("17900001"),
            City = "Duckburg",
            County = "Dade",
            Name = "Date Corp",
            State = "GA",
        },
        new()
        {
            Id = new ApbFacilityId("05900071"),
            City = "Emerald City",
            County = "Early",
            Name = "Elderberry Inc.",
            State = "GA",
        },
        new()
        {
            Id = new ApbFacilityId("05700040"),
            City = "Fer-de-Lance",
            County = "Floyd",
            Name = "Fruit Inc.",
            State = "GA",
        },
    };
}
