using Domain.StackTest.Models.TestRun;
using System.ComponentModel.DataAnnotations;

namespace Domain.StackTest.Models;

public record class StackTestReportTwoStackDre : BaseStackTestReport
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

    // Stacks

    public string StackOneName { get; set; } = "";
    public string StackTwoName { get; set; } = "";

    [Display(Name = "Test runs")]
    public List<TwoStackTestRun> TestRuns { get; set; } = new List<TwoStackTestRun>();

    [Display(Name = "Average pollutant concentration")]
    public ValueWithUnits StackOneAvgPollutantConcentration { get; set; }
    public ValueWithUnits StackTwoAvgPollutantConcentration { get; set; }

    [Display(Name = "Average emission rate")]
    public ValueWithUnits StackOneAvgEmissionRate { get; set; }
    public ValueWithUnits StackTwoAvgEmissionRate { get; set; }

    [Display(Name = "Destruction efficiency (%)")]
    public string DestructionEfficiency { get; set; } = "";

    #region Confidential info handling

    public override StackTestReportTwoStackDre RedactedStackTestReport() =>
        RedactedBaseStackTestReport<StackTestReportTwoStackDre>() with
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
        AddIfConfidential(31, nameof(ApplicableRequirement));
        AddIfConfidential(32, nameof(ControlEquipmentInfo));
        AddIfConfidential(33, nameof(Comments));
        AddIfConfidential(34, nameof(StackOneName));
        AddIfConfidential(35, nameof(StackTwoName));
        AddIfConfidential(79, nameof(StackOneAvgPollutantConcentration));
        AddIfConfidential(80, nameof(StackTwoAvgPollutantConcentration));
        AddIfConfidential(82, nameof(StackOneAvgEmissionRate));
        AddIfConfidential(83, nameof(StackTwoAvgEmissionRate));
        AddIfConfidential(84, nameof(DestructionEfficiency));
    }

    #endregion
}
