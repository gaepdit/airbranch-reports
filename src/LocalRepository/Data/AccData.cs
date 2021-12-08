using Domain.Compliance.Models;

namespace LocalRepository.Data;

public static class AccData
{
    public static IEnumerable<AccReport> GetAccReports => new List<AccReport>
    {
        new() {
            Id = 1,
            AccReportingYear = 2018,
            AllDeviationsReported = true,
            AllTitleVConditionsListed = true,
            Comments = "No compliance issues noted. \r\nThe information in the ACC is consistent " +
            "with previously submitted reports and inspection observations.",
            CorrectFormsUsed = true,
            CorrectlyFilledOut = true,
            DateAcknowledgmentLetterSent = new DateTime(2001, 11, 22),
            DateComplete = new DateTime(2001, 11, 23),
            DatePostmarked = new DateTime(2001, 1, 1),
            DateReceived = new DateTime(2001, 1, 2),
            DeviationsReported = false,
            EnforcementRecommended = false,
            Facility = FacilityData.GetFacilities.Single(e => e.Id.ShortString == "05100149"),
            PostmarkedByDeadline = true,
            ResubmittalRequested = false,
            SignedByResponsibleOfficial = true,
            StaffResponsible = StaffData.GetStaff.Single(e => e.Id == 1).Name,
            UnreportedDeviationsReported = false,
        },
        new() {
            Id = 2,
            AccReportingYear = 2019,
            AllDeviationsReported = false,
            AllTitleVConditionsListed = false,
            Comments = "Deviations reported. Enforcement is recommended.",
            CorrectFormsUsed = false,
            CorrectlyFilledOut = false,
            DateAcknowledgmentLetterSent = null,
            DateComplete = new DateTime(2002, 2, 23),
            DatePostmarked = new DateTime(2002, 2, 2),
            DateReceived = new DateTime(2002, 2, 3),
            DeviationsReported = true,
            EnforcementRecommended = true,
            Facility = FacilityData.GetFacilities.Single(e => e.Id.ShortString == "05100149"),
            PostmarkedByDeadline = false,
            ResubmittalRequested = true,
            SignedByResponsibleOfficial = false,
            StaffResponsible = StaffData.GetStaff.Single(e => e.Id == 2).Name,
            UnreportedDeviationsReported = true,
        },
    };
}
