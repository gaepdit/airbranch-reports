using Dapper;
using Domain.Facilities.Models;
using Domain.Monitoring.Models;
using Domain.Monitoring.Models.StackTestData;
using Domain.Monitoring.Repositories;
using Domain.ValueObjects;
using Infrastructure.Monitoring.Queries;
using System.Data;

namespace Infrastructure.Monitoring;

public class MonitoringRepository : IMonitoringRepository
{
    private readonly IDbConnection db;
    public MonitoringRepository(IDbConnection conn) => db = conn;

    public Task<bool> StackTestReportExistsAsync(ApbFacilityId facilityId, int referenceNumber)
    {
        var query = @"select convert(bit, count(1))
            from dbo.ISMPMASTER
            where STRAIRSNUMBER = @AirsNumber
              and convert(int, STRREFERENCENUMBER) = @ReferenceNumber";

        return db.ExecuteScalarAsync<bool>(query, new
        {
            AirsNumber = facilityId.DbFormattedString,
            ReferenceNumber = referenceNumber,
        });
    }

    public async Task<StackTestReport?> GetStackTestReportAsync(ApbFacilityId facilityId, int referenceNumber)
    {
        if (!await StackTestReportExistsAsync(facilityId, referenceNumber)) return null;

        switch (await GetDocumentTypeAsync(referenceNumber))
        {
            case DocumentType.Unassigned:
                return null;

            case DocumentType.OneStackTwoRuns:
            case DocumentType.OneStackThreeRuns:
            case DocumentType.OneStackFourRuns:
                return await GetOneStackAsync(referenceNumber);

            case DocumentType.TwoStackStandard:
                break;
            case DocumentType.TwoStackDRE:
                break;
            case DocumentType.LoadingRack:
                break;
            case DocumentType.PondTreatment:
                break;
            case DocumentType.GasConcentration:
                break;
            case DocumentType.Flare:
                break;
            case DocumentType.Rata:
                break;
            case DocumentType.MemorandumStandard:
                break;
            case DocumentType.MemorandumToFile:
                break;
            case DocumentType.Method9Multi:
                break;
            case DocumentType.Method22:
                break;
            case DocumentType.Method9Single:
                break;
            case DocumentType.PEMS:
                break;
            case DocumentType.PTEPermanentTotalEnclosure:
                break;

            default:
                return null;
        }
        return null;
    }

    private async Task<T> GetBaseStackTestReportAsync<T>(int referenceNumber) where T : StackTestReport
    {
        using var multi = await db.QueryMultipleAsync(MonitoringQueries.StackTestReport, new { ReferenceNumber = referenceNumber });

        var report = multi.Read<T, Facility, PersonName, PersonName, PersonName, DateRange, T>(
            (report, facility, reviewedByStaff, complianceManager, testingUnitManager, testDates) =>
            {
                report.Facility = facility;
                report.ReviewedByStaff = reviewedByStaff;
                report.ComplianceManager = complianceManager;
                report.TestingUnitManager = testingUnitManager;
                report.TestDates = testDates;
                return report;
            }).Single();

        report.WitnessedByStaff.AddRange(multi.Read<PersonName>());

        return report;
    }

    private async Task<StackTestReportOneStack> GetOneStackAsync(int referenceNumber)
    {
        var report = await GetBaseStackTestReportAsync<StackTestReportOneStack>(referenceNumber);

        using var multi = await db.QueryMultipleAsync(MonitoringQueries.StackTestReportOneStack, new { ReferenceNumber = referenceNumber });

        _ = multi.Read<dynamic, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits, dynamic>(
            (r, maxOperatingCapacity, OperatingCapacity, AvgPollutantConcentration, AvgEmissionRate) =>
            {
                report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                report.PercentAllowable = r.PercentAllowable;
                report.MaxOperatingCapacity = maxOperatingCapacity;
                report.OperatingCapacity = OperatingCapacity;
                report.AvgPollutantConcentration = AvgPollutantConcentration;
                report.AvgEmissionRate = AvgEmissionRate;
                return r;
            });

        report.AllowableEmissionRates.AddRange(multi.Read<ValueWithUnits>());
        report.TestRuns.AddRange(multi.Read<TestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    public Task<DocumentType> GetDocumentTypeAsync(int referenceNumber) =>
        db.QuerySingleAsync<DocumentType>(
            @"select convert(int, STRDOCUMENTTYPE) as DocumentType
            from ISMPREPORTINFORMATION
            where STRREFERENCENUMBER = @ReferenceNumber",
            new { ReferenceNumber = referenceNumber });
}
