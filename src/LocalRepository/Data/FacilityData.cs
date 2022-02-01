namespace LocalRepository.Data;

public static class FacilityData
{
    public static IEnumerable<Facility> GetFacilities => new List<Facility>
    {
        new()
        {
            Id = new ApbFacilityId("00100001"),
            Name = "Apple Corp",
            County = "Appling",
            Description = "Apples and more",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = null,
                City = "Atlantis",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
            HeaderData = new FacilityHeaderData
            {
                OperatingStatusCode = FacilityOperatingStatus.O,
                ClassificationCode = FacilityClassification.A,
                CmsClassificationCode = FacilityCmsClassification.A,
                Sic = "1234",
                Naics = "123456",
                RmpId = null,
                OwnershipType = null,
                StartupDate = new DateTime(2000, 1, 1),
                PermitRevocationDate = null,
                AirPrograms = new List<string> { "SIP", "MACT", },
                ProgramClassifications = new List<string> { "NSR/PSD Major" },
                OneHourOzoneNonattainment = OneHourOzoneNonattainmentStatus.Contribute,
                EightHourOzoneNonattainment = EightHourOzoneNonattainmentStatus.Atlanta,
                PmFineNonattainment = PmFineNonattainmentStatus.Atlanta,
                NspsFeeExempt = false,
            },
        },
        new()
        {
            Id = new ApbFacilityId("12100021"),
            Name = "Banana Corp",
            County = "Bibb",
            Description = "Bananas and more",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite B",
                City = "Bedford Falls",
                State = "GA",
                PostalCode = "30000",
            },
            HeaderData = new FacilityHeaderData
            {
                OperatingStatusCode = FacilityOperatingStatus.O,
                ClassificationCode = FacilityClassification.SM,
                CmsClassificationCode = FacilityCmsClassification.S,
                Sic = "1234",
                Naics = "123456",
                RmpId = "1234-5678-9012",
                OwnershipType = "Federal Facility (U.S. Government)",
                StartupDate = new DateTime(2000, 1, 1),
                PermitRevocationDate = null,
                AirPrograms = new List<string> { "SIP", "NSPS", },
                ProgramClassifications = new List<string>(),
                OneHourOzoneNonattainment = OneHourOzoneNonattainmentStatus.No,
                EightHourOzoneNonattainment = EightHourOzoneNonattainmentStatus.None,
                PmFineNonattainment = PmFineNonattainmentStatus.None,
                NspsFeeExempt = true,
            },
        },
        new()
        {
            Id = new ApbFacilityId("05100149"),
            Name = "Cranberry Corp",
            County = "Clay",
            Description = "Cranberries and more",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = null,
                City = "Coruscant",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
            HeaderData = new FacilityHeaderData
            {
                OperatingStatusCode = FacilityOperatingStatus.X,
                ClassificationCode = FacilityClassification.Unspecified,
                CmsClassificationCode = FacilityCmsClassification.Unspecified,
                Sic = "1234",
                Naics = "123456",
                StartupDate = new DateTime(2000, 1, 1),
                PermitRevocationDate = new DateTime(2020, 2, 2),
                AirPrograms = new List<string> { "SIP" },
                ProgramClassifications = new List<string>(),
                OneHourOzoneNonattainment = OneHourOzoneNonattainmentStatus.No,
                EightHourOzoneNonattainment = EightHourOzoneNonattainmentStatus.None,
                PmFineNonattainment = PmFineNonattainmentStatus.None,
            },
        },
        new()
        {
            Id = new ApbFacilityId("17900001"),
            Name = "Date Corp",
            County = "Dade",
            Description = "Dates and times",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite D",
                City = "Duckburg",
                State = "GA",
                PostalCode = "30000",
            },
        },
        new()
        {
            Id = new ApbFacilityId("05900071"),
            Name = "Elderberry Inc.",
            County = "Early",
            Description = "Your mother was a hamster and your father smelt of elderberries!",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = null,
                City = "Emerald City",
                State = "GA",
                PostalCode = "30000",
            },
        },
        new()
        {
            Id = new ApbFacilityId("05700040"),
            Name = "Fruit Inc.",
            County = "Floyd",
            Description = "Nothing but fruit",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite F",
                City = "Fer-de-Lance",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
        },
        new()
        {
            Id = new ApbFacilityId("00100005"),
            Name = "Guava Inc.",
            County = "Glynn",
            Description = "Guavalicious",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite G",
                City = "Gnu York",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
            HeaderData = new FacilityHeaderData
            {
                OperatingStatusCode = FacilityOperatingStatus.O,
                ClassificationCode = FacilityClassification.SM,
                CmsClassificationCode = FacilityCmsClassification.S,
                Sic = "1234",
                Naics = "123456",
                RmpId = "1234-5678-9012",
                OwnershipType = "Federal Facility (U.S. Government)",
                StartupDate = new DateTime(2000, 1, 1),
                PermitRevocationDate = null,
                AirPrograms = new List<string> { "SIP", "NSPS", },
                ProgramClassifications = new List<string>(),
                OneHourOzoneNonattainment = OneHourOzoneNonattainmentStatus.No,
                EightHourOzoneNonattainment = EightHourOzoneNonattainmentStatus.None,
                PmFineNonattainment = PmFineNonattainmentStatus.None,
                NspsFeeExempt = true,
            },
        },
        new()
        {
            Id = new ApbFacilityId("24500002"),
            Name = "Huckleberry LLC",
            County = "Hall",
            Description = "Huckleberries & Chuckleberries",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite H",
                City = "Hill Valley",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
        },
        new()
        {
            Id = new ApbFacilityId("07300003"),
            Name = "Indian Fig Co.",
            County = "Irwin",
            Description = "Prickly pears",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite I",
                City = "Isthmus City",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
        },
    };
}
