using Domain.Monitoring.Models.StackTestData;
using System.ComponentModel.DataAnnotations;

namespace Domain.Monitoring.Models;

public record class StackTestReportOneStack : StackTestReport
{
    // Operating data

    [Display(Name = "Maximum expected operating capacity")]
    public ValueWithUnits MaxOperatingCapacity { get; init; }

    [Display(Name = "Operating capacity")]
    public ValueWithUnits OperatingCapacity { get; init; }

    [Display(Name = "Allowable emission rates")]
    public List<ValueWithUnits> AllowableEmissionRates { get; init; } = new List<ValueWithUnits>();

    [Display(Name = "Control equipment and monitoring data")]
    public string ControlEquipmentInfo { get; init; } = "";

    // Test run data

    [Display(Name = "Test runs")]
    public List<TestRun> TestRuns { get; init; } = new List<TestRun>();

    [Display(Name = "Average pollutant concentration")]
    public ValueWithUnits AvgPollutantConcentration { get; init; }

    [Display(Name = "Average emission rate")]
    public ValueWithUnits AvgEmissionRate { get; init; }

    [Display(Name = "Percent allowable (%)")]
    public string PercentAllowable { get; init; } = "";

    // Confidential info handling

    // TODO: Add all parameters
    public override StackTestReportOneStack RedactedStackTestReport() =>
        RedactedBaseStackTestReport<StackTestReportOneStack>() with
        {
            ControlEquipmentInfo = CheckConfidential(ControlEquipmentInfo, nameof(ControlEquipmentInfo)),
        };

    public override void ParseConfidentialParameters()
    {
        if (ConfidentialParametersCode == "" || ConfidentialParametersCode[0] == '0') return;

        ParseBaseConfidentialParameters();
        
        // TODO: Fix parsing
        ConfidentialParameters.Add(nameof(ControlEquipmentInfo));
    }
}
