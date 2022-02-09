using Domain.Compliance.Models;

namespace LocalRepository.Data;

public static class AccData
{
    public static IEnumerable<AccReport> AccReports => new List<AccReport>
    {
        new() {
            Id = 1,
            AccReportingYear = 2018,
            AllDeviationsReported = true,
            AllTitleVConditionsListed = true,
            Comments = TextData.LongMultiline,
            CorrectFormsUsed = true,
            CorrectlyFilledOut = true,
            DateAcknowledgmentLetterSent = null,
            DateCompleted = new DateTime(2001, 11, 23),
            DatePostmarked = new DateTime(2001, 1, 1),
            DateReceived = new DateTime(2001, 1, 2),
            DeviationsReported = false,
            EnforcementRecommended = false,
            Facility = FacilityData.GetFacility("05100149"),
            PostmarkedByDeadline = true,
            ResubmittalRequested = false,
            SignedByResponsibleOfficial = true,
            StaffResponsible = StaffData.GetStaff(1)!.Value.Name,
            UnreportedDeviationsReported = false,
        },
        new() {
            Id = 2,
            AccReportingYear = 2019,
            AllDeviationsReported = false,
            AllTitleVConditionsListed = false,
            Comments = TextData.Short,
            CorrectFormsUsed = false,
            CorrectlyFilledOut = false,
            DateAcknowledgmentLetterSent = new DateTime(2001, 11, 22),
            DateCompleted = new DateTime(2002, 2, 23),
            DatePostmarked = new DateTime(2002, 2, 2),
            DateReceived = new DateTime(2002, 2, 3),
            DeviationsReported = true,
            EnforcementRecommended = true,
            Facility = FacilityData.GetFacility("05100149"),
            PostmarkedByDeadline = false,
            ResubmittalRequested = true,
            SignedByResponsibleOfficial = false,
            StaffResponsible = StaffData.GetStaff(2)!.Value.Name,
            UnreportedDeviationsReported = true,
        },
    };
}
