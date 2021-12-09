using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Monitoring.Models;

public abstract record class StackTestReport
{
    // Basic test report info

    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    public Facility Facility { get; init; }

    [Display(Name = "Pollutant determined")]
    public string Pollutant { get; init; } = "";

    [Display(Name = "Source tested")]
    public string Source { get; init; } = "";

    [Display(Name = "Report type")]
    public ReportType ReportType { get; init; }

    [Display(Name = "Document type")]
    public DocumentType DocumentType { get; init; } = DocumentType.Unassigned;

    [Display(Name = "Applicable requirement")]
    public string ApplicableRequirement { get; init; } = "";

    [Display(Name = "Other information")]
    public string Comments { get; init; } = "";

    // Test report routing

    [Display(Name = "Date(s) of test")]
    public DateTimeRange TestDates { get; init; }

    [Display(Name = "Date received by APB")]
    public DateTime DateReceivedByApb { get; init; }

    [Display(Name = "Report reviewed by")]
    public PersonName ReviewedByStaff { get; init; }

    [Display(Name = "Test witnessed by")]
    public List<PersonName> WitnessedByStaff { get; init; } = new List<PersonName>();

    [Display(Name = "Compliance manager")]
    public PersonName ComplianceManager { get; init; }

    [Display(Name = "Testing unit manager")]
    public PersonName TestingUnitManager { get; init; }

    // Confidential info handling

    public List<string> ConfidentialParameters { get; private set; } = new List<string>();

    [JsonIgnore]
    public string ConfidentialParametersCode { protected get; init; } = "";

    protected string CheckConfidential(string input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
        ? GlobalConstants.StackTestConfidentialInfoPlaceholder
        : input;

    public abstract StackTestReport RedactedStackTestReport();
    // TODO: Add all parameters
    protected T RedactedBaseStackTestReport<T>()
        where T : StackTestReport => (T)this with
        {
            Pollutant = CheckConfidential(Pollutant, nameof(Pollutant)),
            Comments = CheckConfidential(Comments, nameof(Comments)),
        };

    public abstract void ParseConfidentialParameters();
    protected void ParseBaseConfidentialParameters()
    {
        // TODO: Fix parsing
        ConfidentialParameters.Add(nameof(Pollutant));
        ConfidentialParameters.Add(nameof(Comments));
    }


}
