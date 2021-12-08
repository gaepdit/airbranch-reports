using Domain.Monitoring.Models.Partials;
using Domain.Personnel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Monitoring.Models;

public record struct StackTestReport
{
    // Basic test report info

    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    public Facility Facility { get; init; }

    [Display(Name = "Pollutant determined")]
    public string Pollutant { get; init; }

    [Display(Name = "Source tested")]
    public string Source { get; init; }

    [Display(Name = "Report type")]
    public ReportType ReportType { get; init; }

    [Display(Name = "Document type")]
    public DocumentType DocumentType { get; init; }

    [Display(Name = "Applicable requirement")]
    public string ApplicableRequirement { get; init; }

    [Display(Name = "Other information")]
    public string Comments { get; init; }

    // Test report routing

    [Display(Name = "Date(s) of test")]
    public DateTimeRange TestDates { get; init; }

    [Display(Name = "Date received by APB")]
    public DateTime DateReceivedByApb { get; init; }

    [Display(Name = "Report reviewed by")]
    public Staff ReviewedByStaff { get; init; }

    [Display(Name = "Test witnessed by")]
    public List<Staff> WitnessedByStaff { get; init; }

    [Display(Name = "Compliance manager")]
    public Staff ComplianceManager { get; init; }

    [Display(Name = "Testing unit manager")]
    public Staff TestingUnitManager { get; init; }

    // Operating data

    [Display(Name = "Maximum expected operating capacity")]
    public ValueWithUnits MaxOperatingCapacity { get; init; }

    [Display(Name = "Operating capacity")]
    public ValueWithUnits OperatingCapacity { get; init; }

    [Display(Name = "Allowable emission rates")]
    public List<ValueWithUnits> AllowableEmissionRates { get; init; }

    [Display(Name = "Control equipment and monitoring data")]
    public string ControlEquipmentInfo { get; init; }

    // Test run data

    [Display(Name = "Test runs")]
    public List<TestRun> TestRuns { get; init; }

    [Display(Name = "Average pollutant concentration")]
    public ValueWithUnits AvgPollutantConcentration { get; init; }

    [Display(Name = "Average emission rate")]
    public ValueWithUnits AvgEmissionRate { get; init; }

    [Display(Name = "Percent allowable (%)")]
    public string PercentAllowable { get; init; }

    // Confidential info handling

    public List<string> ConfidentialParameters { get; private set; }

    [JsonIgnore]
    public string ConfidentialParametersCode { private get; init; }

    private string CheckConfidential(string input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
        ? GlobalConstants.StackTestConfidentialInfoPlaceholder
        : input;

    // TODO: Add all parameters
    public StackTestReport RedactedStackTestReport()
    {
        var redactedStackTestReport = this with
        {
            Pollutant = CheckConfidential(Pollutant, nameof(Pollutant)),
            Comments = CheckConfidential(Comments, nameof(Comments)),
        };

        return redactedStackTestReport;
    }

    // TODO: This is going to get funky
    public void ParseConfidentialParametersCode()
    {
        ConfidentialParameters = new List<string>();

        if (ConfidentialParametersCode == "" || ConfidentialParametersCode[0] == '0') return;

        ConfidentialParameters.Add(nameof(Pollutant));
        ConfidentialParameters.Add(nameof(Comments));
    }
}
