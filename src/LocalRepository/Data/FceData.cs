﻿using Domain.Compliance.Models;
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
                    Comments = TextData.Long,
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
                    Comments = TextData.VeryShort,
                },
            },

            Accs = new List<Acc>
            {
                new()
                {
                    Id = 2,
                    AccReportingYear = 2011,
                    ReceivedDate = new DateTime(2011, 3, 1),
                    Reviewer = StaffData.GetStaff.Single(e => e.Id == 3).Name,
                    DeviationsReported = true,
                },
            },

            Reports = new List<Report>
            {
                new()
                {
                    Id = 200,
                    ReportPeriod = "Second Semiannual",
                    ReportPeriodDateRange = new DateRange(new DateTime(2019, 7, 1), new DateTime(2019, 12, 31)),
                    ReceivedDate = new DateTime(2020, 2, 1),
                    Reviewer = StaffData.GetStaff.Single(e => e.Id == 2).Name,
                    DeviationsReported = true,
                    Comments = TextData.Long,
                },
                new()
                {
                    Id = 300,
                    ReportPeriod = "Annual",
                    ReportPeriodDateRange = new DateRange(new DateTime(2019, 1, 1), new DateTime(2019, 12, 31)),
                    ReceivedDate = new DateTime(2020, 3, 1),
                    Reviewer = StaffData.GetStaff.Single(e => e.Id == 3).Name,
                    DeviationsReported = false,
                    Comments = TextData.Empty,
                },
            },

            Notifications =
            {
                new()
                {
                    Id = 4,
                    DateReceived = new DateTime(2011, 4, 1),
                    Reviewer = StaffData.GetStaff.Single(e => e.Id == 4).Name,
                    Type = "N/A",
                    Comments = TextData.ShortMultiline,
                },
            },

            StackTests =
            {
                new()
                {
                    Id = 80000,
                    ReferenceNumber = 201100001,
                    ReceivedDate = new DateTime(2011, 5, 1),
                    Reviewer = StaffData.GetStaff.Single(e => e.Id == 5).Name,
                    ComplianceStatus = "In Compliance",
                    SourceTested = TextData.Short,
                },
            },

            FeesHistory =
            {
                new()
                {
                    Year = 2011,
                    InvoicedAmount = 100000M,
                    AmountPaid = 99999.99M,
                    Balance = 0.01M,
                    Status = "Partial Payment",
                },
                new()
                {
                    Year = 2010,
                    InvoicedAmount = 4000M,
                    AmountPaid = 4000M,
                    Balance = 0M,
                    Status = "Paid in Full",
                },
            },

            EnforcementHistory =
            {
                new()
                {
                    Id = 1000,
                    StaffResponsible = StaffData.GetStaff.Single(e => e.Id == 6).Name,
                    EnforcementDate = new DateTime(2011, 7, 1),
                    EnforcementType = "Letter of Noncompliance",
                },
            },
        },
        new() {
            Id = 8555,
            FceYear = 2012,
            StaffReviewedBy = StaffData.GetStaff.Single(e => e.Id == 2).Name,
            SupportingDataDateRange = new DateRange(new DateTime(2011, 2, 2), new DateTime(2012, 2, 2)),
            WithOnsiteInspection = false,
            DateCompleted = new DateTime(2012, 2, 28),
            Facility = FacilityData.GetFacilities.Single(e => e.Id?.ShortString == "00100005"),
        },
        new() {
            Id = 3,
            FceYear = 2013,
            StaffReviewedBy = StaffData.GetStaff.Single(e => e.Id == 3).Name,
            SupportingDataDateRange = new DateRange(new DateTime(2012, 3, 3), new DateTime(2013, 3, 3)),
            WithOnsiteInspection = false,
            Comments = TextData.VeryShort,
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