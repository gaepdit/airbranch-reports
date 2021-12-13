using Domain.Monitoring.Models.StackTestData;
using System.ComponentModel.DataAnnotations;

namespace Domain.Monitoring.Models;

public record class StackTestReportOneStack : StackTestReport
{
    // Operating data

    [Display(Name = "Maximum expected operating capacity")]
    public ValueWithUnits MaxOperatingCapacity { get; set; }

    [Display(Name = "Operating capacity")]
    public ValueWithUnits OperatingCapacity { get; set; }

    [Display(Name = "Allowable emission rate(s)")]
    public List<ValueWithUnits> AllowableEmissionRates { get; init; } = new List<ValueWithUnits>();

    [Display(Name = "Control equipment and monitoring data")]
    public string ControlEquipmentInfo { get; set; } = "";

    // Test run data

    [Display(Name = "Test runs")]
    public List<TestRun> TestRuns { get; set; } = new List<TestRun>();

    [Display(Name = "Average pollutant concentration")]
    public ValueWithUnits AvgPollutantConcentration { get; set; }

    [Display(Name = "Average emission rate")]
    public ValueWithUnits AvgEmissionRate { get; set; }

    [Display(Name = "Percent allowable (%)")]
    public string PercentAllowable { get; set; } = "";

    // Confidential info handling

    public override StackTestReportOneStack RedactedStackTestReport() =>
        RedactedBaseStackTestReport<StackTestReportOneStack>() with
        {
            MaxOperatingCapacity = CheckConfidential(MaxOperatingCapacity, nameof(MaxOperatingCapacity)),
            OperatingCapacity = CheckConfidential(OperatingCapacity, nameof(OperatingCapacity)),
            AllowableEmissionRates = CheckConfidential(AllowableEmissionRates, nameof(AllowableEmissionRates)),
            ControlEquipmentInfo = CheckConfidential(ControlEquipmentInfo, nameof(ControlEquipmentInfo)),
            AvgPollutantConcentration = CheckConfidential(AvgPollutantConcentration, nameof(AvgPollutantConcentration)),
            AvgEmissionRate = CheckConfidential(AvgEmissionRate, nameof(AvgEmissionRate)),
            PercentAllowable = CheckConfidential(PercentAllowable, nameof(PercentAllowable)),
            TestRuns = CheckConfidential(TestRuns),
        };

    private static List<TestRun> CheckConfidential(List<TestRun> testRuns)
    {
        var redactedTestRuns = new List<TestRun>();
        foreach (var r in testRuns) redactedTestRuns.Add(r.RedactedTestRun());
        return redactedTestRuns;
    }

    public override void ParseConfidentialParameters()
    {
        if (ConfidentialParametersCode == "" || ConfidentialParametersCode[0] == '0') return;

        ParseBaseConfidentialParameters();

        // TODO: Fix parsing
        ConfidentialParameters.Add(nameof(ControlEquipmentInfo));

        foreach (var r in TestRuns)
        {
            r.ConfidentialParameters.Add(nameof(TestRun.EmissionRate));
        }
    }
}
