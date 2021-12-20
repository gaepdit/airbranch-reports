﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Monitoring.Models;

public record class StackTestReportLoadingRack : StackTestReport
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

    [Display(Name = "Test duration")]
    public ValueWithUnits TestDuration { get; set; }

    [Display(Name = "Pollutant concentration")]
    public ValueWithUnits PollutantConcentrationIn { get; set; }
    public ValueWithUnits PollutantConcentrationOut { get; set; }

    [Display(Name = "Emission rate")]
    public ValueWithUnits EmissionRate { get; set; }

    [Display(Name = "Destruction reduction efficiency")]
    public ValueWithUnits DestructionReduction { get; set; }

    #region Confidential info handling

    public override StackTestReportLoadingRack RedactedStackTestReport() =>
        RedactedBaseStackTestReport<StackTestReportLoadingRack>() with
        {
            MaxOperatingCapacity = CheckConfidential(MaxOperatingCapacity, nameof(MaxOperatingCapacity)),
            OperatingCapacity = CheckConfidential(OperatingCapacity, nameof(OperatingCapacity)),
            AllowableEmissionRates = CheckConfidential(AllowableEmissionRates, nameof(AllowableEmissionRates)),
            ControlEquipmentInfo = CheckConfidential(ControlEquipmentInfo, nameof(ControlEquipmentInfo)),
            TestDuration = CheckConfidential(TestDuration, nameof(TestDuration)),
            PollutantConcentrationIn = CheckConfidential(PollutantConcentrationIn, nameof(PollutantConcentrationIn)),
            PollutantConcentrationOut = CheckConfidential(PollutantConcentrationOut, nameof(PollutantConcentrationOut)),
            EmissionRate = CheckConfidential(EmissionRate, nameof(EmissionRate)),
            DestructionReduction = CheckConfidential(DestructionReduction, nameof(DestructionReduction)),
        };

    public override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        if (ConfidentialParametersCode == "" || ConfidentialParametersCode[0] == '0') return;
        ParseBaseConfidentialParameters();

        AddIfConfidential(26, nameof(MaxOperatingCapacity));
        AddIfConfidential(27, nameof(OperatingCapacity));
        AddIfConfidential(31, nameof(ApplicableRequirement));
        AddIfConfidential(32, nameof(ControlEquipmentInfo));
        AddIfConfidential(33, nameof(TestDuration));
        AddIfConfidential(34, nameof(PollutantConcentrationIn));
        AddIfConfidential(35, nameof(PollutantConcentrationOut));
        AddIfConfidential(36, nameof(DestructionReduction));
        AddIfConfidential(37, nameof(EmissionRate));
        AddIfConfidential(38, nameof(Comments));
    }

    #endregion
}
