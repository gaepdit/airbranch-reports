﻿using System.ComponentModel.DataAnnotations;

namespace Domain.StackTest.Models.TestRun;

public record StackTestRun : BaseTestRun
{
    [Display(Name = "Gas temperature")]
    public string GasTemperature { get; init; } = "";

    [Display(Name = "Gas moisture")]
    public string GasMoisture { get; init; } = "";

    [Display(Name = "Gas flow rate")]
    public string GasFlowRateAcfm { get; init; } = "";

    [Display(Name = "Gas flow rate")]
    public string GasFlowRateDscfm { get; init; } = "";

    [Display(Name = "Pollutant concentration")]
    public string PollutantConcentration { get; init; } = "";

    [Display(Name = "Emission rate")]
    public string EmissionRate { get; init; } = "";

    #region Confidential info handling

    public override StackTestRun RedactedTestRun() =>
        RedactedBaseTestRun<StackTestRun>() with
        {
            GasTemperature = CheckConfidential(GasTemperature, nameof(GasTemperature)),
            GasMoisture = CheckConfidential(GasMoisture, nameof(GasMoisture)),
            GasFlowRateAcfm = CheckConfidential(GasFlowRateAcfm, nameof(GasFlowRateAcfm)),
            GasFlowRateDscfm = CheckConfidential(GasFlowRateDscfm, nameof(GasFlowRateDscfm)),
            PollutantConcentration = CheckConfidential(PollutantConcentration, nameof(PollutantConcentration)),
            EmissionRate = CheckConfidential(EmissionRate, nameof(EmissionRate)),
        };

    protected override void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        if (ConfidentialParametersCode == "") return;
        ParseBaseConfidentialParameters();

        AddIfConfidential(2, nameof(GasTemperature));
        AddIfConfidential(3, nameof(GasMoisture));
        AddIfConfidential(4, nameof(GasFlowRateAcfm));
        AddIfConfidential(5, nameof(GasFlowRateDscfm));
        AddIfConfidential(6, nameof(PollutantConcentration));
        AddIfConfidential(7, nameof(EmissionRate));
    }

    #endregion
}
