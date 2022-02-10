using Dapper;
using Domain.Compliance.Models;
using Domain.Compliance.Models.WorkItems;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using Domain.ValueObjects;
using Infrastructure.Facilities;
using System.Data;

namespace Infrastructure.Compliance;

public class ComplianceRepository : IComplianceRepository
{
    private readonly IDbConnection db;
    public ComplianceRepository(IDbConnection conn) => db = conn;

    // ACC
    public Task<bool> AccReportExistsAsync(ApbFacilityId facilityId, int year) =>
        db.ExecuteScalarAsync<bool>(ComplianceQueries.AccReportExists,
            new
            {
                AirsNumber = facilityId.DbFormattedString,
                Year = year,
            });

    public async Task<AccReport?> GetAccReportAsync(ApbFacilityId facilityId, int year)
    {
        if (!await AccReportExistsAsync(facilityId, year)) return null;

        var param = new
        {
            AirsNumber = facilityId.DbFormattedString,
            Year = year,
        };

        return (await db.QueryAsync<AccReport, Facility, PersonName, AccReport>(
            ComplianceQueries.GetAccReport,
            (a, f, n) =>
            {
                a.Facility = f;
                a.StaffResponsible = n;
                return a;
            },
            param)).Single();
    }

    // FCE
    public Task<bool> FceReportExistsAsync(ApbFacilityId facilityId, int id) =>
        db.ExecuteScalarAsync<bool>(ComplianceQueries.FceReportExists,
            new
            {
                AirsNumber = facilityId.DbFormattedString,
                Id = id,
            });

    public async Task<FceReport?> GetFceReportAsync(ApbFacilityId facilityId, int id)
    {
        if (!await FceReportExistsAsync(facilityId, id)) return null;

        var param = new
        {
            AirsNumber = facilityId.DbFormattedString,
            Id = id,
            Domain.GlobalConstants.FceDataPeriod,
            Domain.GlobalConstants.FceExtendedDataPeriod,
        };

        var facilitiesRepository = new FacilitiesRepository(db);
        var facility = await facilitiesRepository.GetFacilityAsync(facilityId);

        using var multi = await db.QueryMultipleAsync(ComplianceQueries.GetFceReport, param);

        var report = multi.Read<FceReport, PersonName, DateRange, FceReport>(
        (report, staff, dateRange) =>
        {
            report.StaffReviewedBy = staff;
            report.SupportingDataDateRange = dateRange;
            report.Facility = facility;
            return report;
        }).Single();

        report.Inspections.AddRange(multi.Read<Inspection, PersonName, DateRange, Inspection>(
            (item, staff, dateRange) =>
            {
                item.Inspector = staff;
                item.InspectionDate = dateRange;
                return item;
            }));
        report.RmpInspections.AddRange(multi.Read<Inspection, PersonName, DateRange, Inspection>(
            (item, staff, dateRange) =>
            {
                item.Inspector = staff;
                item.InspectionDate = dateRange;
                return item;
            }));
        report.Accs.AddRange(multi.Read<Acc, PersonName, Acc>(
            (item, staff) =>
            {
                item.Reviewer = staff;
                return item;
            }));
        report.Reports.AddRange(multi.Read<Report, PersonName, DateRange, Report>(
            (item, staff, dateRange) =>
            {
                item.Reviewer = staff;
                item.ReportPeriodDateRange = dateRange;
                return item;
            }));
        report.Notifications.AddRange(multi.Read<Notification, PersonName, Notification>(
            (item, staff) =>
            {
                item.Reviewer = staff;
                return item;
            }));
        report.StackTests.AddRange(multi.Read<StackTestWork, PersonName, StackTestWork>(
            (item, staff) =>
            {
                item.Reviewer = staff;
                return item;
            }));
        report.FeesHistory.AddRange(multi.Read<FeeYear>());
        report.EnforcementHistory.AddRange(multi.Read<Enforcement, PersonName, Enforcement>(
            (item, staff) =>
            {
                item.StaffResponsible = staff;
                return item;
            }));

        return report;
    }
}
