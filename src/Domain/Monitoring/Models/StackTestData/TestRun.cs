using System.ComponentModel.DataAnnotations;

namespace Domain.Monitoring.Models.StackTestData;

public record struct TestRun
{
    [Display(Name = "Test run #")]
    public string RunNumber { get; init; } = "";

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

    // Confidential info handling

    public ICollection<string> ConfidentialParameters { get; private set; } = new HashSet<string>();

    public TestRun RedactedTestRun() =>
        this with
        {
            RunNumber = CheckConfidential(RunNumber, nameof(RunNumber)),
            GasTemperature = CheckConfidential(GasTemperature, nameof(GasTemperature)),
            GasMoisture = CheckConfidential(GasMoisture, nameof(GasMoisture)),
            GasFlowRateAscfm = CheckConfidential(GasFlowRateAscfm, nameof(GasFlowRateAscfm)),
            GasFlowRateDscfm = CheckConfidential(GasFlowRateDscfm, nameof(GasFlowRateDscfm)),
            PollutantConcentration = CheckConfidential(PollutantConcentration, nameof(PollutantConcentration)),
            EmissionRate = CheckConfidential(EmissionRate, nameof(EmissionRate)),
        };

    public string CheckConfidential(string input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
        ? GlobalConstants.ConfidentialInfoPlaceholder
        : input;
}
