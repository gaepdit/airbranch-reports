using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Monitoring.Models.StackTestData;

public record struct FlareTestRun
{
    [Display(Name = "Test run #")]
    public string RunNumber { get; init; } = "";

    [Display(Name = "Heating value")]
    public string HeatingValue { get; init; } = "";

    [Display(Name = "Emission rate velocity")]
    public string EmissionRateVelocity { get; init; } = "";

    #region Confidential info handling

    [JsonIgnore]
    public string ConfidentialParametersCode { private get; init; } = "";

    public ICollection<string> ConfidentialParameters { get; private set; } = new HashSet<string>();

    public FlareTestRun RedactedTestRun() =>
        this with
        {
            RunNumber = CheckConfidential(RunNumber, nameof(RunNumber)),
            HeatingValue= CheckConfidential(HeatingValue, nameof(HeatingValue)),
            EmissionRateVelocity= CheckConfidential(EmissionRateVelocity, nameof(EmissionRateVelocity)),
        };

    public string CheckConfidential(string input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
        ? GlobalConstants.ConfidentialInfoPlaceholder
        : input;

    public void ParseConfidentialParameters()
    {
        ConfidentialParameters = new HashSet<string>();
        if (ConfidentialParametersCode == "") return;

        AddIfConfidential(1, nameof(RunNumber));
        AddIfConfidential(2, nameof(HeatingValue));
        AddIfConfidential(3, nameof(EmissionRateVelocity));
    }

    // Uses "ONE"-based position to better correlate with IAIP code
    public void AddIfConfidential(int position, string parameter)
    {
        if (ConfidentialParametersCode[position - 1] == '1') ConfidentialParameters.Add(parameter);
    }

    #endregion
}
