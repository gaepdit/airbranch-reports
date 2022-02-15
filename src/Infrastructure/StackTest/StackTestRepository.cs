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
    // ReSharper disable once InconsistentNaming
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

        return await GetDocumentTypeAsync(referenceNumber) switch
        {
            DocumentType.Unassigned => null,
            DocumentType.OneStackTwoRuns or DocumentType.OneStackThreeRuns or DocumentType.OneStackFourRuns => await GetOneStackAsync(referenceNumber),
            DocumentType.TwoStackStandard or DocumentType.TwoStackDre => await GetTwoStackAsync(referenceNumber),
            DocumentType.LoadingRack => await GetLoadingRackAsync(referenceNumber),
            DocumentType.PondTreatment => await GetPondTreatmentAsync(referenceNumber),
            DocumentType.GasConcentration => await GetGasConcentrationAsync(referenceNumber),
            DocumentType.Flare => await GetFlareAsync(referenceNumber),
            DocumentType.Rata => await GetRataAsync(referenceNumber),
            DocumentType.MemorandumStandard or DocumentType.MemorandumToFile or DocumentType.PTE => await GetMemorandumAsync(referenceNumber),
            DocumentType.Method9Multi or DocumentType.Method22 or DocumentType.Method9Single => await GetOpacityAsync(referenceNumber),
            _ => null,
        };
    }

    private async Task<T> GetBaseStackTestReportAsync<T>(int referenceNumber) where T : BaseStackTestReport
    {
        using var multi = await db.QueryMultipleAsync(StackTestQueries.BaseStackTestReport,
            new { ReferenceNumber = referenceNumber });

        var report = multi.Read<T, Facility, Address, PersonName, PersonName, PersonName, DateRange, T>(
            (report, facility, address, reviewedByStaff, complianceManager, testingUnitManager, testDates) =>
            {
                report.Facility = facility;
                report.Facility.FacilityAddress = address;
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

        using var multi = await db.QueryMultipleAsync(StackTestQueries.StackTestReportOneStack,
            new { ReferenceNumber = referenceNumber });

        _ = multi
            .Read<StackTestReportOneStack, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits,
                StackTestReportOneStack>(
                (r, maxOperatingCapacity, operatingCapacity, avgPollutantConcentration, avgEmissionRate) =>
                {
                    report.MaxOperatingCapacity = maxOperatingCapacity;
                    report.OperatingCapacity = operatingCapacity;
                    report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                    report.AvgPollutantConcentration = avgPollutantConcentration;
                    report.AvgEmissionRate = avgEmissionRate;
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
            new[]
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
            results =>
            {
                var r = (StackTestReportTwoStack)results[0];
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
            new[]
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
            results =>
            {
                var r = (StackTestReportLoadingRack)results[0];
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

        _ = multi
            .Read<StackTestReportPondTreatment, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits,
                StackTestReportPondTreatment>(
                (r, maxOperatingCapacity, operatingCapacity, avgPollutantCollectionRate, avgTreatmentRate) =>
                {
                    report.MaxOperatingCapacity = maxOperatingCapacity;
                    report.OperatingCapacity = operatingCapacity;
                    report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                    report.AvgPollutantCollectionRate = avgPollutantCollectionRate;
                    report.AvgTreatmentRate = avgTreatmentRate;
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

        _ = multi
            .Read<StackTestReportGasConcentration, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits,
                StackTestReportGasConcentration>(
                (r, maxOperatingCapacity, operatingCapacity, avgPollutantConcentration, avgEmissionRate) =>
                {
                    report.MaxOperatingCapacity = maxOperatingCapacity;
                    report.OperatingCapacity = operatingCapacity;
                    report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                    report.AvgPollutantConcentration = avgPollutantConcentration;
                    report.AvgEmissionRate = avgEmissionRate;
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

        _ = multi
            .Read<StackTestReportFlare, ValueWithUnits, ValueWithUnits, ValueWithUnits, ValueWithUnits,
                StackTestReportFlare>(
                (r, maxOperatingCapacity, operatingCapacity, avgHeatingValue, avgEmissionRateVelocity) =>
                {
                    report.MaxOperatingCapacity = maxOperatingCapacity;
                    report.OperatingCapacity = operatingCapacity;
                    report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                    report.AvgHeatingValue = avgHeatingValue;
                    report.AvgEmissionRateVelocity = avgEmissionRateVelocity;
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

    private async Task<StackTestMemorandum> GetMemorandumAsync(int referenceNumber)
    {
        var report = await GetBaseStackTestReportAsync<StackTestMemorandum>(referenceNumber);

        using var multi = await db.QueryMultipleAsync(StackTestQueries.StackTestMemorandum,
            new { ReferenceNumber = referenceNumber });

        _ = multi.Read<StackTestMemorandum, ValueWithUnits, ValueWithUnits, StackTestMemorandum>(
            (r, maxOperatingCapacity, operatingCapacity) =>
            {
                report.MonitorManufacturer = r.MonitorManufacturer;
                report.MonitorSerialNumber = r.MonitorSerialNumber;
                report.MaxOperatingCapacity = maxOperatingCapacity;
                report.OperatingCapacity = operatingCapacity;
                report.ControlEquipmentInfo = r.ControlEquipmentInfo;
                report.Comments = r.Comments;
                return r;
            });

        report.AllowableEmissionRates.AddRange(multi.Read<ValueWithUnits>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestReportOpacity> GetOpacityAsync(int referenceNumber)
    {
        var report = await GetBaseStackTestReportAsync<StackTestReportOpacity>(referenceNumber);

        using var multi = await db.QueryMultipleAsync(StackTestQueries.StackTestReportOpacity,
            new { ReferenceNumber = referenceNumber });

        var r = await multi.ReadSingleAsync<StackTestReportOpacity>();

        report.ControlEquipmentInfo = r.ControlEquipmentInfo;
        report.ComplianceStatus = r.ComplianceStatus;
        report.OpacityStandard = r.OpacityStandard;
        report.TestDuration = r.TestDuration;
        report.MaxOperatingCapacityUnits = r.MaxOperatingCapacityUnits;
        report.OperatingCapacityUnits = r.OperatingCapacityUnits;
        report.AllowableEmissionRateUnits = r.AllowableEmissionRateUnits;

        report.TestRuns.AddRange(multi.Read<OpacityTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }
}
