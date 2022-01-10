using Domain.Compliance.Models;
using Domain.Compliance.Models.WorkItems;

namespace LocalRepository.Data;

public static class FceData
{
    public static IEnumerable<FceReport> GetFceReports => new List<FceReport>
    {
        new() {
            Id = 7136,
            FceYear = 2011,
            StaffReviewedBy = StaffData.GetStaff.Single(e => e.Id == 1).Name,
            SupportingDataDateRange = new DateRange(new DateTime(2010, 1, 1), new DateTime(2011, 1, 1)),
            WithOnsiteInspection = true,
            Comments = "In compliance.",
            DateCompleted = new DateTime(2011, 1, 30),
            Facility = FacilityData.GetFacilities.Single(e => e.Id?.ShortString == "00100001"),

            Inspections = new List<Inspection>
            {
                new()
                {
                    Id = 1,
                    Inspector = StaffData.GetStaff.Single(e => e.Id == 1).Name,
                    InspectionDate = new DateRange {StartDate = new DateTime(2011, 1, 1)},
                    Reason = "Planned Unannounced",
                    FacilityWasOperating = true,
                    ComplianceStatus = "Compliant",
                },
            },

            RmpInspections = new List<RmpInspection>
            {
                new()
                {
                    Id = 40001,
                    Inspector = StaffData.GetStaff.Single(e => e.Id == 2).Name,
                    InspectionDate = new DateRange {StartDate = new DateTime(2011, 2, 2)},
                    Reason = "Planned Unannounced",
                    FacilityWasOperating = true,
                    ComplianceStatus = "Compliant",
                },
            },

            Reports = new List<Report>
            {
                new()
                {
                    Id = 200,
                    ReportPeriod = "Second Semiannual",
                    ReportPeriodDates = new DateRange(new DateTime(2019, 7, 1), new DateTime(2019, 12, 31)),
                    ReceivedDate = new DateTime(2020, 2, 1),
                    Reviewer = StaffData.GetStaff.Single(e => e.Id == 2).Name,
                    DeviationsReported = true,
                    Comments = "In compliance. And what's more, not out of compliance. " +
                    "In other words, vis a vis compliance status, more in than out. " +
                    "And I guess that's all I have to say about that.",
                },
                new()
                {
                    Id = 300,
                    ReportPeriod = "Annual",
                    ReportPeriodDates = new DateRange(new DateTime(2019, 1, 1), new DateTime(2019, 12, 31)),
                    ReceivedDate = new DateTime(2020, 3, 1),
                    Reviewer = StaffData.GetStaff.Single(e => e.Id == 3).Name,
                    DeviationsReported = false,
                    Comments = "",
                },
            }
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
        new() {
            Id = 3,
            FceYear = 2013,
            StaffReviewedBy = StaffData.GetStaff.Single(e => e.Id == 3).Name,
            SupportingDataDateRange = new DateRange(new DateTime(2012, 3, 3), new DateTime(2013, 3, 3)),
            WithOnsiteInspection = false,
            Comments = "N/A",
            DateCompleted = new DateTime(2013, 3, 30),
            Facility = FacilityData.GetFacilities.Single(e => e.Id?.ShortString == "00100001"),

            Inspections = new List<Inspection>
            {
                new()
                {
                    Id = 11111,
                    Inspector = StaffData.GetStaff.Single(e => e.Id == 3).Name,
                    InspectionDate = new DateRange {StartDate = new DateTime(2011, 1, 1)},
                    Reason = "Joint EPD/EPA",
                    FacilityWasOperating = false,
                    ComplianceStatus = "Deviation(s) Noted",
                },
                new()
                {
                    Id = 22222,
                    Inspector = StaffData.GetStaff.Single(e => e.Id == 4).Name,
                    InspectionDate = new DateRange(new DateTime(2013, 3, 1), new DateTime(2013, 3, 3)),
                    Reason = "Complaint Investigation",
                    FacilityWasOperating = true,
                    ComplianceStatus = "Compliant",
                },
            },
        },
    };
}
