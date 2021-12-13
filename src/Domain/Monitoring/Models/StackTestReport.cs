using Domain.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Monitoring.Models;

public abstract record class StackTestReport
{
    // Basic test report info

    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    public Facility Facility { get; set; }

    [Display(Name = "Pollutant determined")]
    public string Pollutant { get; init; } = "";

    [Display(Name = "Source tested")]
    public string Source { get; init; } = "";

    [JsonIgnore]
    public ReportType ReportType { get; init; }

    [Display(Name = "Report type")]
    public string ReportTypeName => ReportType.GetDescription();

    [JsonIgnore]
    public string ReportTypeSubject => ReportType switch
    {
        ReportType.MonitorCertification => "Monitor Certification",
        ReportType.PemsDevelopment => "PEMS Development Report Review",
        ReportType.RataCems => "Relative Accuracy Test Audit Report Review",
        ReportType.SourceTest => "Source Test Report Review",
        _ => "N/A",
    };

    [Display(Name = "Document type")]
    public DocumentType DocumentType { get; init; } = DocumentType.Unassigned;

    [Display(Name = "Applicable requirement")]
    public string ApplicableRequirement { get; init; } = "";

    [Display(Name = "Other information")]
    public string Comments { get; init; } = "";

    // Test report routing

    [Display(Name = "Date(s) of test")]
    public DateRange TestDates { get; set; }

    [Display(Name = "Date received by APB")]
    public DateTime DateReceivedByApb { get; init; }

    [Display(Name = "Report reviewed by")]
    public PersonName ReviewedByStaff { get; set; }

    [Display(Name = "Test witnessed by")]
    public List<PersonName> WitnessedByStaff { get; set; } = new List<PersonName>();

    [Display(Name = "Compliance manager")]
    public PersonName ComplianceManager { get; set; }

    [Display(Name = "Testing unit manager")]
    public PersonName TestingUnitManager { get; set; }

    // Confidential info handling

    public ICollection<string> ConfidentialParameters { get; private set; } = new List<string>();

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
