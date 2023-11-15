using Domain.Compliance.Models;

namespace LocalRepository.Data;

public static class AccData
{
    public static IEnumerable<AccReport> AccReports => new List<AccReport>
    {
        new()
        {
            Id = 77863,
            AccReportingYear = 2018,
            AllDeviationsReported = true,
            AllTitleVConditionsListed = true,
            Comments = TextData.LongMultiline,
            CorrectFormsUsed = true,
            CorrectlyFilledOut = true,
            DateComplete = null,
            DatePostmarked = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Local),
            DateReceived = new DateTime(2001, 1, 2, 0, 0, 0, DateTimeKind.Local),
            DeviationsReported = false,
            EnforcementRecommended = false,
            Facility = FacilityData.GetFacility("05100149"),
            PostmarkedByDeadline = true,
            ResubmittalRequested = false,
            SignedByResponsibleOfficial = true,
            StaffResponsible = StaffData.GetStaff(1)!.Value.Name,
            UnreportedDeviationsReported = false,
        },
        new()
        {
            Id = 83966,
            AccReportingYear = 2019,
            AllDeviationsReported = false,
            AllTitleVConditionsListed = false,
            Comments = TextData.Short,
            CorrectFormsUsed = false,
            CorrectlyFilledOut = false,
            DateComplete = new DateTime(2001, 11, 22, 0, 0, 0, DateTimeKind.Local),
            DatePostmarked = new DateTime(2002, 2, 2, 0, 0, 0, DateTimeKind.Local),
            DateReceived = new DateTime(2002, 2, 3, 0, 0, 0, DateTimeKind.Local),
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
