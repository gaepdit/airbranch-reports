using Dapper;
using Domain.Facilities.Models;
using Domain.StackTest.Models;
using Domain.StackTest.Models.TestRun;
using Domain.StackTest.Repositories;
using Domain.ValueObjects;
using System.Data;

namespace Infrastructure.StackTest;

public class StackTestRepository : IStackTestRepository
{
    private readonly IDbConnection db;
    public StackTestRepository(IDbConnection conn) => db = conn;

    public Task<bool> StackTestReportExistsAsync(ApbFacilityId facilityId, int referenceNumber) =>
        db.ExecuteScalarAsync<bool>(StackTestQueries.StackTestReportExists,
            new
            {
                AirsNumber = facilityId.DbFormattedString,
                ReferenceNumber = referenceNumber,
            });

    public Task<DocumentType> GetDocumentTypeAsync(int referenceNumber) =>
        db.QuerySingleAsync<DocumentType>(StackTestQueries.GetDocumentType,
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
                return await GetTwoStackAsync(referenceNumber);

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
        using var multi = await db.QueryMultipleAsync(StackTestQueries.BaseStackTestReport,
            new { ReferenceNumber = referenceNumber });

        var report = multi.Read<T, Facility, Address, PersonName, PersonName, PersonName, DateRange, T>(
            (report, Facility, Address, ReviewedByStaff, ComplianceManager, TestingUnitManager, TestDates) =>
            {
                report.Facility = Facility;
                report.Facility.FacilityAddress = Address;
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

        using var multi = await db.QueryMultipleAsync(StackTestQueries.StackTestReportOneStack,
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

    private async Task<StackTestReportTwoStack> GetTwoStackAsync(int referenceNumber)
    {
        var report = await GetBaseStackTestReportAsync<StackTestReportTwoStack>(referenceNumber);

        using var multi = await db.QueryMultipleAsync(StackTestQueries.StackTestReportTwoStack,
            new { ReferenceNumber = referenceNumber });

        _ = multi.Read(
            types: new[]
            {
                typeof(StackTestReportTwoStack),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
                typeof(ValueWithUnits),
            },
            map: results =>
            {
                StackTestReportTwoStack r = (StackTestReportTwoStack)results[0];
                report.MaxOperatingCapacity = (ValueWithUnits)results[1];
                report.OperatingCapacity = (ValueWithUnits)results[2];
                report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                report.StackOneName = r.StackOneName;
                report.StackTwoName = r.StackTwoName;
                report.StackOneAvgPollutantConcentration = (ValueWithUnits)results[3];
                report.StackTwoAvgPollutantConcentration = (ValueWithUnits)results[4];
                report.StackOneAvgEmissionRate = (ValueWithUnits)results[5];
                report.StackTwoAvgEmissionRate = (ValueWithUnits)results[6];
                report.SumAvgEmissionRate = (ValueWithUnits)results[7];
                report.PercentAllowable = r.PercentAllowable;
                return r;
            });

        report.AllowableEmissionRates.AddRange(multi.Read<ValueWithUnits>());

        report.TestRuns.AddRange(multi.Read<TwoStackTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestReportLoadingRack> GetLoadingRackAsync(int referenceNumber)
    {
        var report = await GetBaseStackTestReportAsync<StackTestReportLoadingRack>(referenceNumber);

        using var multi = await db.QueryMultipleAsync(StackTestQueries.StackTestReportLoadingRack,
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

    private async Task<StackTestReportFlare> GetFlareAsync(int referenceNumber)
    {
        var report = await GetBaseStackTestReportAsync<StackTestReportFlare>(referenceNumber);

        using var multi = await db.QueryMultipleAsync(StackTestQueries.StackTestReportFlare,
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
}
