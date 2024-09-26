using Dapper;
using Domain.Facilities.Models;
using Domain.StackTest.Models;
using Domain.StackTest.Models.TestRun;
using Domain.StackTest.Repositories;
using Domain.ValueObjects;
using Infrastructure.DbConnection;
using System.Data;

namespace Infrastructure.StackTest;

public class StackTestRepository(IDbConnectionFactory dbf) : IStackTestRepository
{
    public async Task<bool> StackTestReportExistsAsync(FacilityId facilityId, int referenceNumber)
    {
        using var db = dbf.Create();
        return await db.ExecuteScalarAsync<bool>("air.StackTestReportExists",
            new
            {
                AirsNumber = facilityId.DbFormattedString,
                ReferenceNumber = referenceNumber,
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<DocumentType> GetDocumentTypeAsync(int referenceNumber)
    {
        using var db = dbf.Create();
        return await db.QuerySingleAsync<DocumentType>("air.GetStackTestDocumentType",
            new { ReferenceNumber = referenceNumber },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<BaseStackTestReport?> GetStackTestReportAsync(FacilityId facilityId, int referenceNumber)
    {
        var getDocumentTypeTask = GetDocumentTypeAsync(referenceNumber);
        var getStackTestReportExistsTask = StackTestReportExistsAsync(facilityId, referenceNumber);

        if (!await getStackTestReportExistsTask) return null;

        return await getDocumentTypeTask switch
        {
            DocumentType.Unassigned => null,
            DocumentType.OneStackTwoRuns or DocumentType.OneStackThreeRuns or DocumentType.OneStackFourRuns =>
                await GetOneStackAsync(referenceNumber),
            DocumentType.TwoStackStandard or DocumentType.TwoStackDre => await GetTwoStackAsync(referenceNumber),
            DocumentType.LoadingRack => await GetLoadingRackAsync(referenceNumber),
            DocumentType.PondTreatment => await GetPondTreatmentAsync(referenceNumber),
            DocumentType.GasConcentration => await GetGasConcentrationAsync(referenceNumber),
            DocumentType.Flare => await GetFlareAsync(referenceNumber),
            DocumentType.Rata => await GetRataAsync(referenceNumber),
            DocumentType.MemorandumStandard or DocumentType.MemorandumToFile or DocumentType.PTE =>
                await GetMemorandumAsync(referenceNumber),
            DocumentType.Method9Multi or DocumentType.Method22 or DocumentType.Method9Single => await GetOpacityAsync(
                referenceNumber),
            _ => null,
        };
    }

    private async Task<T> GetBaseStackTestReportAsync<T>(int referenceNumber) where T : BaseStackTestReport
    {
        using var db = dbf.Create();

        await using var multi = await db.QueryMultipleAsync("air.GetBaseStackTestReport",
            new { ReferenceNumber = referenceNumber },
            commandType: CommandType.StoredProcedure);

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

        report.WitnessedByStaff.AddRange(await multi.ReadAsync<PersonName>());

        return report;
    }

    private async Task<StackTestReportOneStack> GetOneStackAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetStackTestReportOneStack",
            new { ReferenceNumber = referenceNumber },
            commandType: CommandType.StoredProcedure);

        var report = await GetBaseStackTestReportAsync<StackTestReportOneStack>(referenceNumber);

        await using var multi = await getMultiTask;
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

        report.AllowableEmissionRates.AddRange(await multi.ReadAsync<ValueWithUnits>());
        report.TestRuns.AddRange(await multi.ReadAsync<StackTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestReportTwoStack> GetTwoStackAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetStackTestReportTwoStack",
            new { ReferenceNumber = referenceNumber },
            commandType: CommandType.StoredProcedure);

        var report = await GetBaseStackTestReportAsync<StackTestReportTwoStack>(referenceNumber);
        await using var multi = await getMultiTask;

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

        report.AllowableEmissionRates.AddRange(await multi.ReadAsync<ValueWithUnits>());
        report.TestRuns.AddRange(await multi.ReadAsync<TwoStackTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestReportLoadingRack> GetLoadingRackAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetStackTestReportLoadingRack",
            new { ReferenceNumber = referenceNumber },
            commandType: CommandType.StoredProcedure);

        var report = await GetBaseStackTestReportAsync<StackTestReportLoadingRack>(referenceNumber);

        await using var multi = await getMultiTask;
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

        report.AllowableEmissionRates.AddRange(await multi.ReadAsync<ValueWithUnits>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestReportPondTreatment> GetPondTreatmentAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetStackTestReportPondTreatment",
            new { ReferenceNumber = referenceNumber },
            commandType: CommandType.StoredProcedure);

        var report = await GetBaseStackTestReportAsync<StackTestReportPondTreatment>(referenceNumber);

        await using var multi = await getMultiTask;
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

        report.TestRuns.AddRange(await multi.ReadAsync<PondTreatmentTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestReportGasConcentration> GetGasConcentrationAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetStackTestReportGasConcentration",
            new { ReferenceNumber = referenceNumber },
            commandType: CommandType.StoredProcedure);

        var report = await GetBaseStackTestReportAsync<StackTestReportGasConcentration>(referenceNumber);

        await using var multi = await getMultiTask;
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

        report.AllowableEmissionRates.AddRange(await multi.ReadAsync<ValueWithUnits>());
        report.TestRuns.AddRange(await multi.ReadAsync<GasConcentrationTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestReportFlare> GetFlareAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetStackTestReportFlare",
            new { ReferenceNumber = referenceNumber },
            commandType: CommandType.StoredProcedure);

        var report = await GetBaseStackTestReportAsync<StackTestReportFlare>(referenceNumber);

        await using var multi = await getMultiTask;
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

        report.AllowableEmissionRates.AddRange(await multi.ReadAsync<ValueWithUnits>());
        report.TestRuns.AddRange(await multi.ReadAsync<FlareTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestReportRata> GetRataAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetStackTestReportRata",
            new { ReferenceNumber = referenceNumber },
            commandType: CommandType.StoredProcedure);

        var report = await GetBaseStackTestReportAsync<StackTestReportRata>(referenceNumber);

        await using var multi = await getMultiTask;
        var r = await multi.ReadSingleAsync<StackTestReportRata>();

        report.ApplicableStandard = r.ApplicableStandard;
        report.Diluent = r.Diluent;
        report.Units = r.Units;
        report.RelativeAccuracyCode = r.RelativeAccuracyCode;
        report.RelativeAccuracyPercent = r.RelativeAccuracyPercent;
        report.RelativeAccuracyRequiredPercent = r.RelativeAccuracyRequiredPercent;
        report.RelativeAccuracyRequiredLabel = r.RelativeAccuracyRequiredLabel;
        report.ComplianceStatus = r.ComplianceStatus;

        report.TestRuns.AddRange(await multi.ReadAsync<RataTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestMemorandum> GetMemorandumAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetStackTestMemorandum",
            new { ReferenceNumber = referenceNumber },
            commandType: CommandType.StoredProcedure);

        var report = await GetBaseStackTestReportAsync<StackTestMemorandum>(referenceNumber);

        await using var multi = await getMultiTask;
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

        report.AllowableEmissionRates.AddRange(await multi.ReadAsync<ValueWithUnits>());

        report.ParseConfidentialParameters();
        return report;
    }

    private async Task<StackTestReportOpacity> GetOpacityAsync(int referenceNumber)
    {
        using var db = dbf.Create();

        var getMultiTask = db.QueryMultipleAsync("air.GetStackTestReportOpacity",
            new { ReferenceNumber = referenceNumber },
            commandType: CommandType.StoredProcedure);

        var report = await GetBaseStackTestReportAsync<StackTestReportOpacity>(referenceNumber);

        await using var multi = await getMultiTask;
        var r = await multi.ReadSingleAsync<StackTestReportOpacity>();

        report.ControlEquipmentInfo = r.ControlEquipmentInfo;
        report.ComplianceStatus = r.ComplianceStatus;
        report.OpacityStandard = r.OpacityStandard;
        report.TestDuration = r.TestDuration;
        report.MaxOperatingCapacityUnits = r.MaxOperatingCapacityUnits;
        report.OperatingCapacityUnits = r.OperatingCapacityUnits;
        report.AllowableEmissionRateUnits = r.AllowableEmissionRateUnits;

        report.TestRuns.AddRange(await multi.ReadAsync<OpacityTestRun>());

        report.ParseConfidentialParameters();
        return report;
    }
}
