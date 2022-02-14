using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.StackTest.Models.TestRun;

public abstract record BaseTestRun
{
    [Display(Name = "Test run #")]
    public string RunNumber { get; init; } = "";

    #region Confidential info handling

    [JsonIgnore]
    public string ConfidentialParametersCode { protected get; init; } = "";

    public ICollection<string> ConfidentialParameters { get; protected set; } = new HashSet<string>();

    public abstract BaseTestRun RedactedTestRun();

    protected T RedactedBaseTestRun<T>() where T : BaseTestRun =>
        (T)this with
        {
            RunNumber = CheckConfidential(RunNumber, nameof(RunNumber)),
        };

    protected string CheckConfidential(string input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
            ? GlobalConstants.ConfidentialInfoPlaceholder
            : input;

    protected ValueWithUnits CheckConfidential(ValueWithUnits input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
            ? new ValueWithUnits(GlobalConstants.ConfidentialInfoPlaceholder, input.Units, input.Preamble)
            : input;

    protected abstract void ParseConfidentialParameters();

    protected void ParseBaseConfidentialParameters()
    {
        AddIfConfidential(1, nameof(RunNumber));
    }

    // Uses "ONE"-based position to better correlate with IAIP code
    protected void AddIfConfidential(int position, string parameter)
    {
        if (ConfidentialParametersCode.Length < position) return;
        if (ConfidentialParametersCode[position - 1] == '1') ConfidentialParameters.Add(parameter);
    }

    internal static List<T> RedactedTestRuns<T>(List<T> testRuns) where T : BaseTestRun
    {
        var redactedTestRuns = new List<T>();
        foreach (var r in testRuns) redactedTestRuns.Add((T)r.RedactedTestRun());
        return redactedTestRuns;
    }

    internal static List<T> ParsedTestRuns<T>(List<T> testRuns) where T : BaseTestRun
    {
        var parsedTestRuns = new List<T>();
        foreach (var r in testRuns)
        {
            r.ParseConfidentialParameters();
            parsedTestRuns.Add(r);
        }

        return parsedTestRuns;
    }

    #endregion
}
