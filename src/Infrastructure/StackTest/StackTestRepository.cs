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
            case DocumentType.TwoStackDre:
                return await GetTwoStackAsync(referenceNumber);

            case DocumentType.LoadingRack:
                return await GetLoadingRackAsync(referenceNumber);

            case DocumentType.PondTreatment:
                return await GetPondTreatmentAsync(referenceNumber);

            case DocumentType.GasConcentration:
                return await GetGasConcentrationAsync(referenceNumber);

            case DocumentType.Flare:
                return await GetFlareAsync(referenceNumber);

            case DocumentType.Rata:
                return await GetRataAsync(referenceNumber);

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

        _ = multi.Read<StackTestReportOneStack, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits, StackTestReportOneStack>(
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
                report.DestructionEfficiency = r.DestructionEfficiency;
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

        _ = multi.Read(
            types: new[]
            {
                typeof(StackTestReportLoadingRack),
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
                StackTestReportLoadingRack r = (StackTestReportLoadingRack)results[0];
                report.MaxOperatingCapacity = (ValueWithUnits)results[1];
                report.OperatingCapacity = (ValueWithUnits)results[2];
                report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                report.TestDuration = (ValueWithUnits)results[3];
                report.PollutantConcentrationIn = (ValueWithUnits)results[4];
                report.PollutantConcentrationOut = (ValueWithUnits)results[5];
                report.EmissionRate = (ValueWithUnits)results[6];
                report.DestructionReduction = (ValueWithUnits)results[7];
                return r;
            });

        report.AllowableEmissionRates.AddRange(multi.Read<ValueWithUnits>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestReportPondTreatment> GetPondTreatmentAsync(int referenceNumber)
    {
        var report = await GetBaseStackTestReportAsync<StackTestReportPondTreatment>(referenceNumber);

        using var multi = await db.QueryMultipleAsync(StackTestQueries.StackTestReportPondTreatment,
            new { ReferenceNumber = referenceNumber });

        _ = multi.Read<StackTestReportPondTreatment, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits, StackTestReportPondTreatment>(
            (r, MaxOperatingCapacity, OperatingCapacity, AvgPollutantCollectionRate, AvgTreatmentRate) =>
            {
                report.MaxOperatingCapacity = MaxOperatingCapacity;
                report.OperatingCapacity = OperatingCapacity;
                report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                report.AvgPollutantCollectionRate = AvgPollutantCollectionRate;
                report.AvgTreatmentRate = AvgTreatmentRate;
                report.DestructionEfficiency = r.DestructionEfficiency;
                return r;
            });

        report.TestRuns.AddRange(multi.Read<PondTreatmentTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestReportGasConcentration> GetGasConcentrationAsync(int referenceNumber)
    {
        var report = await GetBaseStackTestReportAsync<StackTestReportGasConcentration>(referenceNumber);

        using var multi = await db.QueryMultipleAsync(StackTestQueries.StackTestReportGasConcentration,
            new { ReferenceNumber = referenceNumber });

        _ = multi.Read<StackTestReportGasConcentration, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits, StackTestReportGasConcentration>(
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
        report.TestRuns.AddRange(multi.Read<GasConcentrationTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestReportFlare> GetFlareAsync(int referenceNumber)
    {
        var report = await GetBaseStackTestReportAsync<StackTestReportFlare>(referenceNumber);

        using var multi = await db.QueryMultipleAsync(StackTestQueries.StackTestReportFlare,
            new { ReferenceNumber = referenceNumber });

        _ = multi.Read<StackTestReportFlare, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits, StackTestReportFlare>(
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

    private async Task<StackTestReportRata> GetRataAsync(int referenceNumber)
    {
        var report = await GetBaseStackTestReportAsync<StackTestReportRata>(referenceNumber);

        using var multi = await db.QueryMultipleAsync(StackTestQueries.StackTestReportRata,
            new { ReferenceNumber = referenceNumber });

        var r = await multi.ReadSingleAsync<StackTestReportRata>();

        report.ApplicableStandard = r.ApplicableStandard;
        report.Diluent = r.Diluent;
        report.Units = r.Units;
        report.RelativeAccuracyCode = r.RelativeAccuracyCode;
        report.RelativeAccuracyPercent = r.RelativeAccuracyPercent;
        report.RelativeAccuracyRequiredPercent = r.RelativeAccuracyRequiredPercent;
        report.RelativeAccuracyRequiredLabel = r.RelativeAccuracyRequiredLabel;
        report.ComplianceStatus = r.ComplianceStatus;
        report.TestRuns.AddRange(multi.Read<RataTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }
}
