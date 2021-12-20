using System.ComponentModel.DataAnnotations;

namespace Domain.Monitoring.Models.TestRuns;

public record class StackTestRun : BaseTestRun
{
    [Display(Name = "Gas temperature (°F)")]
    public string GasTemperature { get; init; } = "";

    [Display(Name = "Gas moisture (%)")]
    public string GasMoisture { get; init; } = "";

    [Display(Name = "Gas flow rate (ASCFM)")]
    public string GasFlowRateAscfm { get; init; } = "";

    [Display(Name = "Gas flow rate (DSCFM)")]
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
           GasFlowRateAscfm = CheckConfidential(GasFlowRateAscfm, nameof(GasFlowRateAscfm)),
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
        AddIfConfidential(4, nameof(GasFlowRateAscfm));
        AddIfConfidential(5, nameof(GasFlowRateDscfm));
        AddIfConfidential(6, nameof(PollutantConcentration));
        AddIfConfidential(7, nameof(EmissionRate));
    }

    #endregion
}
