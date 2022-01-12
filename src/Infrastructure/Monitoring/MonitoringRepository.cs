using Dapper;
using Domain.Facilities.Models;
using Domain.Monitoring.Models;
using Domain.Monitoring.Models.TestRuns;
using Domain.Monitoring.Repositories;
using Domain.ValueObjects;
using System.Data;

namespace Infrastructure.Monitoring;

public class MonitoringRepository : IMonitoringRepository
{
    private readonly IDbConnection db;
    public MonitoringRepository(IDbConnection conn) => db = conn;

    public Task<bool> StackTestReportExistsAsync(ApbFacilityId facilityId, int referenceNumber) =>
        db.ExecuteScalarAsync<bool>(MonitoringQueries.StackTestReportExists,
            new
            {
                AirsNumber = facilityId.DbFormattedString,
                ReferenceNumber = referenceNumber,
            });

    public Task<DocumentType> GetDocumentTypeAsync(int referenceNumber) =>
        db.QuerySingleAsync<DocumentType>(MonitoringQueries.GetDocumentType,
            new { ReferenceNumber = referenceNumber });

    public async Task<BaseStackTestReport?> GetStackTestReportAsync(ApbFacilityId facilityId, int referenceNumber)
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
                return await GetLoadingRackAsync(referenceNumber);

            case DocumentType.PondTreatment:
                break;
            case DocumentType.GasConcentration:
                break;
            case DocumentType.Flare:
                return await GetFlareAsync(referenceNumber);

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
            case DocumentType.PTE:
                break;

            default:
                return null;
        }
        return null;
    }

    private async Task<T> GetBaseStackTestReportAsync<T>(int referenceNumber) where T : BaseStackTestReport
    {
        using var multi = await db.QueryMultipleAsync(MonitoringQueries.BaseStackTestReport,
            new { ReferenceNumber = referenceNumber });

        var report = multi.Read<T, Facility, PersonName, PersonName, PersonName, DateRange, T>(
            (report, Facility, ReviewedByStaff, ComplianceManager, TestingUnitManager, TestDates) =>
            {
                report.Facility = Facility;
                report.ReviewedByStaff = ReviewedByStaff;
                report.ComplianceManager = ComplianceManager;
                report.TestingUnitManager = TestingUnitManager;
                report.TestDates = TestDates;
                return report;
            }).Single();

        report.WitnessedByStaff.AddRange(multi.Read<PersonName>());

        return report;
    }

    private async Task<StackTestReportOneStack> GetOneStackAsync(int referenceNumber)
    {
        var report = await GetBaseStackTestReportAsync<StackTestReportOneStack>(referenceNumber);

        using var multi = await db.QueryMultipleAsync(MonitoringQueries.StackTestReportOneStack,
            new { ReferenceNumber = referenceNumber });

        _ = multi.Read<dynamic, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits, dynamic>(
            (r, MaxOperatingCapacity, OperatingCapacity, AvgPollutantConcentration, AvgEmissionRate) =>
            {
                report.MaxOperatingCapacity = MaxOperatingCapacity;
                report.OperatingCapacity = OperatingCapacity;
                report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                report.AvgPollutantConcentration = AvgPollutantConcentration;
                report.AvgEmissionRate = AvgEmissionRate;
                report.PercentAllowable = r.PercentAllowable;
                return r;
            });

        report.AllowableEmissionRates.AddRange(multi.Read<ValueWithUnits>());
        report.TestRuns.AddRange(multi.Read<StackTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestReportFlare> GetFlareAsync(int referenceNumber)
    {
        var report = await GetBaseStackTestReportAsync<StackTestReportFlare>(referenceNumber);

        using var multi = await db.QueryMultipleAsync(MonitoringQueries.StackTestReportFlare,
            new { ReferenceNumber = referenceNumber });

        _ = multi.Read<dynamic, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits, dynamic>(
            (r, MaxOperatingCapacity, OperatingCapacity, AvgHeatingValue, AvgEmissionRateVelocity) =>
            {
                report.MaxOperatingCapacity = MaxOperatingCapacity;
                report.OperatingCapacity = OperatingCapacity;
                report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                report.AvgHeatingValue = AvgHeatingValue;
                report.AvgEmissionRateVelocity = AvgEmissionRateVelocity;
                report.PercentAllowable = r.PercentAllowable;
                return r;
            });

        report.AllowableEmissionRates.AddRange(multi.Read<ValueWithUnits>());
        report.TestRuns.AddRange(multi.Read<FlareTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestReportLoadingRack> GetLoadingRackAsync(int referenceNumber)
    {
        var report = await GetBaseStackTestReportAsync<StackTestReportLoadingRack>(referenceNumber);

        using var multi = await db.QueryMultipleAsync(MonitoringQueries.StackTestReportLoadingRack,
            new { ReferenceNumber = referenceNumber });

        _ = multi.Read<dynamic, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits, dynamic>(
            (r, MaxOperatingCapacity, OperatingCapacity, TestDuration, PollutantConcentrationIn, PollutantConcentrationOut, EmissionRate) =>
            {
                report.MaxOperatingCapacity = MaxOperatingCapacity;
                report.OperatingCapacity = OperatingCapacity;
                report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                report.TestDuration = TestDuration;
                report.PollutantConcentrationIn = PollutantConcentrationIn;
                report.PollutantConcentrationOut = PollutantConcentrationOut;
                report.EmissionRate = EmissionRate;
                report.DestructionReduction = new ValueWithUnits(r.DestructionReduction, "%");
                return r;
            });

        report.AllowableEmissionRates.AddRange(multi.Read<ValueWithUnits>());

        report.ParseConfidentialParameters();
        return report;
    }
}
