﻿using Domain.Compliance.Models;
using Domain.Compliance.Models.WorkItems;

namespace LocalRepository.Data;

public static class FceData
{
    public static IEnumerable<FceReport> FceReports => new List<FceReport>
    {
        new()
        {
            Id = 7136,
            FceYear = 2011,
            StaffReviewedBy = StaffData.GetStaff(1)!.Value.Name,
            SupportingDataDateRange = new DateRange(
                new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2011, 1, 1, 0, 0, 0, DateTimeKind.Local)
            ),
            WithOnsiteInspection = true,
            Comments = "In compliance.",
            DateCompleted = new DateTime(2011, 1, 30, 0, 0, 0, DateTimeKind.Local),
            Facility = FacilityData.GetFacility("00100001"),

            Inspections =
            [
                new Inspection
                {
                    Id = 1,
                    Inspector = StaffData.GetStaff(1)!.Value.Name,
                    InspectionDate =
                        new DateRange { StartDate = new DateTime(2011, 1, 1, 0, 0, 0, DateTimeKind.Local) },
                    Reason = "Planned Unannounced",
                    FacilityWasOperating = true,
                    ComplianceStatus = "Compliant",
                    Comments = TextData.Long,
                }
            ],

            RmpInspections =
            [
                new Inspection
                {
                    Id = 40001,
                    Inspector = StaffData.GetStaff(2)!.Value.Name,
                    InspectionDate =
                        new DateRange { StartDate = new DateTime(2011, 2, 2, 0, 0, 0, DateTimeKind.Local) },
                    Reason = "Planned Unannounced",
                    FacilityWasOperating = true,
                    ComplianceStatus = "Compliant",
                    Comments = TextData.VeryShort,
                }
            ],

            Accs =
            [
                new Acc
                {
                    Id = 2,
                    AccReportingYear = 2011,
                    ReceivedDate = new DateTime(2011, 3, 1, 0, 0, 0, DateTimeKind.Local),
                    Reviewer = StaffData.GetStaff(3)!.Value.Name,
                    DeviationsReported = true,
                }
            ],

            Reports =
            [
                new Report
                {
                    Id = 200,
                    ReportPeriod = "Second Semiannual",
                    ReportPeriodDateRange = new DateRange(
                        new DateTime(2019, 7, 1, 0, 0, 0, DateTimeKind.Local),
                        new DateTime(2019, 12, 31, 0, 0, 0, DateTimeKind.Local)
                    ),
                    ReceivedDate = new DateTime(2020, 2, 1, 0, 0, 0, DateTimeKind.Local),
                    Reviewer = StaffData.GetStaff(2)!.Value.Name,
                    DeviationsReported = true,
                    Comments = TextData.Long,
                },

                new Report
                {
                    Id = 300,
                    ReportPeriod = "Annual",
                    ReportPeriodDateRange = new DateRange(
                        new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Local),
                        new DateTime(2019, 12, 31, 0, 0, 0, DateTimeKind.Local)
                    ),
                    ReceivedDate = new DateTime(2020, 3, 1, 0, 0, 0, DateTimeKind.Local),
                    Reviewer = StaffData.GetStaff(3)!.Value.Name,
                    DeviationsReported = false,
                    Comments = TextData.Empty,
                }
            ],

            Notifications =
            {
                new Notification
                {
                    Id = 4,
                    ReceivedDate = new DateTime(2011, 4, 1, 0, 0, 0, DateTimeKind.Local),
                    Reviewer = StaffData.GetStaff(4)!.Value.Name,
                    Type = "N/A",
                    Comments = TextData.ShortMultiline,
                },
            },

            StackTests =
            {
                new StackTestWork
                {
                    Id = 80000,
                    ReferenceNumber = 201100001,
                    ReceivedDate = new DateTime(2011, 5, 1, 0, 0, 0, DateTimeKind.Local),
                    Reviewer = StaffData.GetStaff(5)!.Value.Name,
                    ComplianceStatus = "In Compliance",
                    SourceTested = TextData.Short,
                },
            },

            FeesHistory =
            {
                new FeeYear
                {
                    Year = 2011,
                    InvoicedAmount = 100000M,
                    AmountPaid = 99999.99M,
                    Balance = 0.01M,
                    Status = "Partial Payment",
                },
                new FeeYear
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
                new Enforcement
                {
                    Id = 1000,
                    StaffResponsible = StaffData.GetStaff(6)!.Value.Name,
                    EnforcementDate = new DateTime(2011, 7, 1, 0, 0, 0, DateTimeKind.Local),
                    EnforcementType = "Letter of Noncompliance",
                },
            },
        },
        new()
        {
            Id = 8555,
            FceYear = 2012,
            StaffReviewedBy = StaffData.GetStaff(2)!.Value.Name,
            SupportingDataDateRange = new DateRange(
                new DateTime(2011, 2, 2, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2012, 2, 2, 0, 0, 0, DateTimeKind.Local)
            ),
            WithOnsiteInspection = false,
            DateCompleted = new DateTime(2012, 2, 28, 0, 0, 0, DateTimeKind.Local),
            Facility = FacilityData.GetFacility("00100005"),
        },
        new()
        {
            Id = 3,
            FceYear = 2013,
            StaffReviewedBy = StaffData.GetStaff(3)!.Value.Name,
            SupportingDataDateRange = new DateRange(
                new DateTime(2012, 3, 3, 0, 0, 0, DateTimeKind.Local),
                new DateTime(2013, 3, 3, 0, 0, 0, DateTimeKind.Local)
            ),
            WithOnsiteInspection = false,
            Comments = TextData.None,
            DateCompleted = new DateTime(2013, 3, 30, 0, 0, 0, DateTimeKind.Local),
            Facility = FacilityData.GetFacility("00100001"),

            Inspections =
            [
                new Inspection
                {
                    Id = 11111,
                    Inspector = StaffData.GetStaff(3)!.Value.Name,
                    InspectionDate =
                        new DateRange { StartDate = new DateTime(2011, 1, 1, 0, 0, 0, DateTimeKind.Local) },
                    Reason = "Joint EPD/EPA",
                    FacilityWasOperating = false,
                    ComplianceStatus = "Deviation(s) Noted",
                },

                new Inspection
                {
                    Id = 22222,
                    Inspector = StaffData.GetStaff(4)!.Value.Name,
                    InspectionDate = new DateRange(
                        new DateTime(2013, 3, 1, 0, 0, 0, DateTimeKind.Local),
                        new DateTime(2013, 3, 3, 0, 0, 0, DateTimeKind.Local)
                    ),
                    Reason = "Complaint Investigation",
                    FacilityWasOperating = true,
                    ComplianceStatus = "Compliant",
                }
            ],
        },
    };
}
