using Dapper;
using Domain;
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
    // ReSharper disable once InconsistentNaming
    private readonly IDbConnection db;
    public ComplianceRepository(IDbConnection conn) => db = conn;

    // ACC
    public async Task<AccReport?> GetAccReportAsync(ApbFacilityId facilityId, int year)
    {
        var param = new
        {
            AirsNumber = facilityId.DbFormattedString,
            Year = year,
        };

        if (!await db.ExecuteScalarAsync<bool>("air.AccReportExists",
                param, commandType: CommandType.StoredProcedure))
            return null;

        return (await db.QueryAsync<AccReport, Facility, PersonName, AccReport>(
                "air.GetAccReport",
                (report, facility, staff) =>
                {
                    report.Facility = facility;
                    report.StaffResponsible = staff;
                    return report;
                },
                param, commandType: CommandType.StoredProcedure))
            .Single();
    }

    // FCE
    public async Task<FceReport?> GetFceReportAsync(ApbFacilityId facilityId, int id)
    {
        var existParam = new
        {
            AirsNumber = facilityId.DbFormattedString,
            Id = id,
        };

        if (!await db.ExecuteScalarAsync<bool>("air.FceReportExists",
                existParam, commandType: CommandType.StoredProcedure))
            return null;

        var facilitiesRepository = new FacilitiesRepository(db);
        var facility = await facilitiesRepository.GetFacilityAsync(facilityId);

        var param = new
        {
            AirsNumber = facilityId.DbFormattedString,
            Id = id,
            GlobalConstants.FceDataPeriod,
            GlobalConstants.FceExtendedDataPeriod,
        };

        using var multi = await db.QueryMultipleAsync("air.GetFceReport",
            param, commandType: CommandType.StoredProcedure);

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
