using Domain.StackTest.Models.TestRun;
using System.ComponentModel.DataAnnotations;

namespace Domain.StackTest.Models;

public record StackTestReportTwoStack : BaseStackTestReport
{
    // Operating data

    [Display(Name = "Maximum expected operating capacity")]
    public ValueWithUnits MaxOperatingCapacity { get; set; }

    [Display(Name = "Operating capacity")]
    public ValueWithUnits OperatingCapacity { get; set; }

    [Display(Name = "Allowable emission rate(s)")]
    public List<ValueWithUnits> AllowableEmissionRates { get; init; } = new();

    [Display(Name = "Control equipment and monitoring data")]
    public string ControlEquipmentInfo { get; set; } = "";

    // Stacks

    public string StackOneName { get; set; } = "";
    public string StackTwoName { get; set; } = "";

    [Display(Name = "Test runs")]
    public List<TwoStackTestRun> TestRuns { get; set; } = new();

    [Display(Name = "Average pollutant concentration")]
    public ValueWithUnits StackOneAvgPollutantConcentration { get; set; }

    public ValueWithUnits StackTwoAvgPollutantConcentration { get; set; }

    [Display(Name = "Average emission rate")]
    public ValueWithUnits StackOneAvgEmissionRate { get; set; }

    public ValueWithUnits StackTwoAvgEmissionRate { get; set; }

    // `SumAvgEmissionRate` is only used by Two Stack (Standard)
    [Display(Name = "Total")]
    public ValueWithUnits SumAvgEmissionRate { get; set; }

    // `PercentAllowable` is only used by Two Stack (Standard)
    [Display(Name = "Percent allowable (%)")]
    public string PercentAllowable { get; set; } = "";

    // `DestructionEfficiency` is only used by Two Stack (DRE)
    [Display(Name = "Destruction efficiency (%)")]
    public string DestructionEfficiency { get; set; } = "";

    #region Confidential info handling

    public override StackTestReportTwoStack RedactedStackTestReport() =>
        RedactedBaseStackTestReport<StackTestReportTwoStack>() with
        {
            MaxOperatingCapacity = CheckConfidential(MaxOperatingCapacity, nameof(MaxOperatingCapacity)),
            OperatingCapacity = CheckConfidential(OperatingCapacity, nameof(OperatingCapacity)),
            ControlEquipmentInfo = CheckConfidential(ControlEquipmentInfo, nameof(ControlEquipmentInfo)),
            StackOneName = CheckConfidential(StackOneName, nameof(StackOneName)),
            StackTwoName = CheckConfidential(StackTwoName, nameof(StackTwoName)),
            StackOneAvgPollutantConcentration = CheckConfidential(StackOneAvgPollutantConcentration, nameof(StackOneAvgPollutantConcentration)),
            StackTwoAvgPollutantConcentration = CheckConfidential(StackTwoAvgPollutantConcentration, nameof(StackTwoAvgPollutantConcentration)),
            StackOneAvgEmissionRate = CheckConfidential(StackOneAvgEmissionRate, nameof(StackOneAvgEmissionRate)),
            StackTwoAvgEmissionRate = CheckConfidential(StackTwoAvgEmissionRate, nameof(StackTwoAvgEmissionRate)),
            SumAvgEmissionRate = CheckConfidential(SumAvgEmissionRate, nameof(SumAvgEmissionRate)),
            PercentAllowable = CheckConfidential(PercentAllowable, nameof(PercentAllowable)),
            DestructionEfficiency = CheckConfidential(DestructionEfficiency, nameof(DestructionEfficiency)),
            TestRuns = BaseTestRun.RedactedTestRuns(TestRuns),
        };

    public override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        TestRuns = BaseTestRun.ParsedTestRuns(TestRuns);

        if (ConfidentialParametersCode == "" || ConfidentialParametersCode[0] == '0') return;
        ParseBaseConfidentialParameters();

        AddIfConfidential(26, nameof(MaxOperatingCapacity));
        AddIfConfidential(27, nameof(OperatingCapacity));
        AddIfConfidential(32, nameof(ControlEquipmentInfo));
        AddIfConfidential(33, nameof(Comments));
        AddIfConfidential(34, nameof(StackOneName));
        AddIfConfidential(35, nameof(StackTwoName));
        AddIfConfidential(79, nameof(StackOneAvgPollutantConcentration));
        AddIfConfidential(80, nameof(StackTwoAvgPollutantConcentration));
        AddIfConfidential(82, nameof(StackOneAvgEmissionRate));
        AddIfConfidential(83, nameof(StackTwoAvgEmissionRate));
        AddIfConfidential(84, nameof(DestructionEfficiency));
        AddIfConfidential(87, nameof(SumAvgEmissionRate));
        AddIfConfidential(88, nameof(PercentAllowable));
    }

    #endregion
}
