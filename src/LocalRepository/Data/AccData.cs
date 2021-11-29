using Domain.Compliance.Models;

namespace LocalRepository.Data;

internal static class AccData
{
    public static IEnumerable<Acc> GetAccs => new List<Acc>
    {
        new() {
            Id = 1,
            AccReportingYear = 2001,
            AllDeviationsReported = true,
            AllTitleVConditionsListed = true,
            CorrectFormsUsed = true,
            CorrectlyFilledOut = true,
            DateAcknowledgmentLetterSent = new DateTime(2001, 11, 22),
            DateComplete = new DateTime(2001, 11, 23),
            DatePostmarked = new DateTime(2001, 1, 1),
            DateReceived = new DateTime(2001, 1, 2),
            DeviationsReported = false,
            EnforcementNeeded = false,
            Facility = FacilityData.GetFacilities.Single(e => e.Id.ShortString == "00100001"),
            PostmarkedByDeadline = true,
            ResubmittalRequested = false,
            SignedByResponsibleOfficial = true,
            StaffResponsible = StaffData.GetStaff.Single(e => e.Id == 1),
            UnreportedDeviationsReported = false,
        },
        new() {
            Id = 2,
            AccReportingYear = 2002,
            AllDeviationsReported = false,
            AllTitleVConditionsListed = false,
            CorrectFormsUsed = false,
            CorrectlyFilledOut = false,
            DateAcknowledgmentLetterSent = new DateTime(2002, 2, 22),
            DateComplete = new DateTime(2002, 2, 23),
            DatePostmarked = new DateTime(2002, 2, 2),
            DateReceived = new DateTime(2002, 2, 3),
            DeviationsReported = true,
            EnforcementNeeded = true,
            Facility = FacilityData.GetFacilities.Single(e => e.Id.ShortString == "00200002"),
            PostmarkedByDeadline = false,
            ResubmittalRequested = true,
            SignedByResponsibleOfficial = false,
            StaffResponsible = StaffData.GetStaff.Single(e => e.Id == 2),
            UnreportedDeviationsReported = true,
        },
    };
}
