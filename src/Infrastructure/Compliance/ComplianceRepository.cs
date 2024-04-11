using Dapper;
using Domain;
using Domain.Compliance.Models;
using Domain.Compliance.Models.WorkItems;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using Domain.Facilities.Repositories;
using Domain.ValueObjects;
using Infrastructure.DbConnection;
using System.Data;

namespace Infrastructure.Compliance;

public class ComplianceRepository(IDbConnectionFactory dbf, IFacilitiesRepository facilitiesRepository)
    : IComplianceRepository
{
    // ACC
    public async Task<AccReport?> GetAccReportAsync(ApbFacilityId facilityId, int id)
    {
        var getReportExistsTask = ReportExistsAsync("air.AccReportExists", facilityId, id);
        var getAccReportTask = GetAccReportDocumentAsync(facilityId, id);

        if (!await getReportExistsTask) return null;
        return await getAccReportTask;
    }

    private async Task<AccReport?> GetAccReportDocumentAsync(ApbFacilityId facilityId, int id)
    {
        var param = new
        {
            AirsNumber = facilityId.DbFormattedString,
            Id = id,
        };

        using var db = dbf.Create();

        return (await db.QueryAsync<AccReport, Facility, PersonName, AccReport>(
            "air.GetAccReport",
            (report, facility, staff) =>
            {
                report.Facility = facility;
                report.StaffResponsible = staff;
                return report;
            },
            param, commandType: CommandType.StoredProcedure)).Single();
    }

    private async Task<bool> ReportExistsAsync(string proc, ApbFacilityId facilityId, int reportId)
    {
        var param = new
        {
            AirsNumber = facilityId.DbFormattedString,
            Id = reportId,
        };

        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<bool>(proc, param, commandType: CommandType.StoredProcedure);
    }

    // FCE
    public async Task<FceReport?> GetFceReportAsync(ApbFacilityId facilityId, int id)
    {
        var getFacilityTask = facilitiesRepository.GetFacilityAsync(facilityId);

        var param = new
        {
            AirsNumber = facilityId.DbFormattedString,
            Id = id,
            GlobalConstants.FceDataPeriod,
            GlobalConstants.FceExtendedDataPeriod,
        };

        using var db = dbf.Create();

        var getFceReportTask = db.QueryMultipleAsync("air.GetFceReport",
            param, commandType: CommandType.StoredProcedure);

        if (!await ReportExistsAsync("air.FceReportExists", facilityId, id)) return null;

        await using var multi = await getFceReportTask;

        var facility = await getFacilityTask;

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
