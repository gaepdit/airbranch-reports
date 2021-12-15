using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

    [JsonIgnore]
    public string ConfidentialParametersCode { private get; init; } = "";

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

    public void ParseConfidentialParameters()
    {
        if (ConfidentialParametersCode == "") return;

        AddIfConfidential(1, nameof(RunNumber));
        AddIfConfidential(2, nameof(GasTemperature));
        AddIfConfidential(3, nameof(GasMoisture));
        AddIfConfidential(4, nameof(GasFlowRateAscfm));
        AddIfConfidential(5, nameof(GasFlowRateDscfm));
        AddIfConfidential(6, nameof(PollutantConcentration));
        AddIfConfidential(7, nameof(EmissionRate));
    }

    // Uses "ONE"-based position to better correlate with IAIP code
    public void AddIfConfidential(int position, string parameter)
    {
        if (ConfidentialParametersCode[position - 1] == '1') ConfidentialParameters.Add(parameter);
    }
}
