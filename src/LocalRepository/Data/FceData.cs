using Domain.Compliance.Models;

namespace LocalRepository.Data;

public static class FceData
{
    public static IEnumerable<FceReport> GetFceReports => new List<FceReport>
    {
        new() {
            Id = 1,
            FceYear = 2011,
            StaffReviewedBy = StaffData.GetStaff.Single(e => e.Id == 1).Name,
            SupportingDataDateRange = new DateRange(new DateTime(2010, 1, 1), new DateTime(2011, 1, 1)),
            WithOnsiteInspection = true,
            Comments = "In compliance.",
            DateCompleted = new DateTime(2011, 1, 30),
            Facility = FacilityData.GetFacilities.Single(e => e.Id?.ShortString == "00100001"),
        },
        new() {
            Id = 2,
            FceYear = 2012,
            StaffReviewedBy = StaffData.GetStaff.Single(e => e.Id == 2).Name,
            SupportingDataDateRange = new DateRange(new DateTime(2011, 2, 2), new DateTime(2012, 2, 2)),
            WithOnsiteInspection = false,
            DateCompleted = new DateTime(2012, 2, 28),
            Facility = FacilityData.GetFacilities.Single(e => e.Id?.ShortString == "05100149"),
        },
    };
}
