using Domain.StackTest.Models.TestRun;
using System.ComponentModel.DataAnnotations;

namespace Domain.StackTest.Models;

public record class StackTestReportOneStack : BaseStackTestReport
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
    public List<StackTestRun> TestRuns { get; set; } = new List<StackTestRun>();

    [Display(Name = "Average pollutant concentration")]
    public ValueWithUnits AvgPollutantConcentration { get; set; }

    [Display(Name = "Average emission rate")]
    public ValueWithUnits AvgEmissionRate { get; set; }

    [Display(Name = "Percent allowable (%)")]
    public string PercentAllowable { get; set; } = "";

    #region Confidential info handling

    public override StackTestReportOneStack RedactedStackTestReport() =>
        RedactedBaseStackTestReport<StackTestReportOneStack>() with
        {
            MaxOperatingCapacity = CheckConfidential(MaxOperatingCapacity, nameof(MaxOperatingCapacity)),
            OperatingCapacity = CheckConfidential(OperatingCapacity, nameof(OperatingCapacity)),
            ControlEquipmentInfo = CheckConfidential(ControlEquipmentInfo, nameof(ControlEquipmentInfo)),
            AvgPollutantConcentration = CheckConfidential(AvgPollutantConcentration, nameof(AvgPollutantConcentration)),
            AvgEmissionRate = CheckConfidential(AvgEmissionRate, nameof(AvgEmissionRate)),
            PercentAllowable = CheckConfidential(PercentAllowable, nameof(PercentAllowable)),
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
        AddIfConfidential(33, nameof(PercentAllowable));
        AddIfConfidential(34, nameof(Comments));

        switch (DocumentType)
        {
            case DocumentType.OneStackTwoRuns:
                AddIfConfidential(49, nameof(AvgPollutantConcentration));
                AddIfConfidential(50, nameof(AvgPollutantConcentration));
                AddIfConfidential(51, nameof(AvgEmissionRate));
                AddIfConfidential(52, nameof(AvgEmissionRate));
                break;

            case DocumentType.OneStackThreeRuns:
                AddIfConfidential(56, nameof(AvgPollutantConcentration));
                AddIfConfidential(57, nameof(AvgPollutantConcentration));
                AddIfConfidential(58, nameof(AvgEmissionRate));
                AddIfConfidential(59, nameof(AvgEmissionRate));
                break;

            case DocumentType.OneStackFourRuns:
                AddIfConfidential(63, nameof(AvgPollutantConcentration));
                AddIfConfidential(64, nameof(AvgPollutantConcentration));
                AddIfConfidential(65, nameof(AvgEmissionRate));
                AddIfConfidential(66, nameof(AvgEmissionRate));
                break;

            default:
                break;
        }

    }

    #endregion
}
