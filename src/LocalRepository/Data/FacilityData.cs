namespace LocalRepository.Data;

public static class FacilityData
{
    public static Facility? GetFacility(string facilityId) =>
        Facilities.SingleOrDefault(e => e.Id == new ApbFacilityId(facilityId));

    public static Facility? GetFacility(ApbFacilityId facilityId) =>
        Facilities.SingleOrDefault(e => e.Id == facilityId);

    public static IEnumerable<Facility> Facilities => new List<Facility>
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
                StartupDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local),
                PermitRevocationDate = null,
                AirPrograms = ["SIP", "MACT"],
                ProgramClassifications = ["NSR/PSD Major"],
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
                StartupDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local),
                PermitRevocationDate = null,
                AirPrograms = ["SIP", "NSPS"],
                ProgramClassifications = [],
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
                StartupDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local),
                PermitRevocationDate = new DateTime(2020, 2, 2, 0, 0, 0, DateTimeKind.Local),
                AirPrograms = ["SIP"],
                ProgramClassifications = [],
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
                StartupDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local),
                PermitRevocationDate = null,
                AirPrograms = ["SIP", "NSPS"],
                ProgramClassifications = [],
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
        new()
        {
            Id = new ApbFacilityId("11500021"),
            Name = "Juniper Berry Co.",
            County = "Jones",
            Description = "Genièvre",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite J",
                City = "Jump City",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
        },
        new()
        {
            Id = new ApbFacilityId("15300040"),
            Name = "Lingonberry LLC",
            County = "Lee",
            Description = "Lingonberries",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite L",
                City = "Lost City of Atlanta",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
        },
        new()
        {
            Id = new ApbFacilityId("30500001"),
            Name = "Muscadine Inc.",
            County = "McIntosh",
            Description = "Jellies and Jams",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite M",
                City = "Maycomb",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
        },
        new()
        {
            Id = new ApbFacilityId("31300062"),
            Name = "Nectarine Corp.",
            County = "Newton",
            Description = "Nectarines and More",
            FacilityAddress = new Address
            {
                Street = "123 Main Street",
                Street2 = "Suite N",
                City = "North Haverbrook",
                State = "GA",
                PostalCode = "30000",
            },
            GeoCoordinates = new GeoCoordinates(34.1M, -84.5M),
        },
    };
}
