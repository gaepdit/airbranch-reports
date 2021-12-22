using Domain.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Monitoring.Models;

public abstract record class BaseStackTestReport
{
    // Basic test report info

    [Display(Name = "Reference Number")]
    public int ReferenceNumber { get; init; }

    public Facility? Facility { get; set; }

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

    [JsonIgnore]
    public DocumentType DocumentType { get; init; } = DocumentType.Unassigned;

    [Display(Name = "Document type")]
    public string DocumentTypeName => DocumentType.GetDescription();

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

    #region Confidential info handling

    // For documentation of the ConfidentialParametersCode string, see:
    // https://github.com/gaepdit/iaip/blob/main/IAIP/ISMP/ISMPConfidentialData.vb

    [JsonIgnore]
    public string ConfidentialParametersCode { protected get; init; } = "";

    public ICollection<string> ConfidentialParameters { get; protected set; } = new HashSet<string>();

    public abstract BaseStackTestReport RedactedStackTestReport();

    protected T RedactedBaseStackTestReport<T>() where T : BaseStackTestReport =>
        (T)this with
        {
            Pollutant = CheckConfidential(Pollutant, nameof(Pollutant)),
            Source = CheckConfidential(Source, nameof(Source)),
            ApplicableRequirement = CheckConfidential(ApplicableRequirement, nameof(ApplicableRequirement)),
            Comments = CheckConfidential(Comments, nameof(Comments)),
            TestDates = CheckConfidential(TestDates, nameof(TestDates)),
            DateReceivedByApb = CheckConfidential(DateReceivedByApb, nameof(DateReceivedByApb)),
            ReviewedByStaff = CheckConfidential(ReviewedByStaff, nameof(ReviewedByStaff)),
            WitnessedByStaff = CheckConfidential(WitnessedByStaff, nameof(WitnessedByStaff)),
            ComplianceManager = CheckConfidential(ComplianceManager, nameof(ComplianceManager)),
            TestingUnitManager = CheckConfidential(TestingUnitManager, nameof(TestingUnitManager)),
        };

    protected string CheckConfidential(string input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
        ? GlobalConstants.ConfidentialInfoPlaceholder
        : input;

    protected DateTime CheckConfidential(DateTime input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
        ? default
        : input;

    protected DateRange CheckConfidential(DateRange input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
        ? new DateRange(default, null)
        : input;

    protected PersonName CheckConfidential(PersonName input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
        ? new PersonName("", GlobalConstants.ConfidentialInfoPlaceholder)
        : input;

    protected List<PersonName> CheckConfidential(List<PersonName> input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
        ? new List<PersonName> { new PersonName("", GlobalConstants.ConfidentialInfoPlaceholder) }
        : input;

    protected ValueWithUnits CheckConfidential(ValueWithUnits input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
        ? new ValueWithUnits(GlobalConstants.ConfidentialInfoPlaceholder, input.Units, input.Preamble)
        : input;

    protected List<ValueWithUnits> CheckConfidential(List<ValueWithUnits> input, string parameter) =>
        ConfidentialParameters.Contains(parameter)
        ? new List<ValueWithUnits> { new ValueWithUnits(GlobalConstants.ConfidentialInfoPlaceholder, "") }
        : input;

    public abstract void ParseConfidentialParameters();
    protected void ParseBaseConfidentialParameters()
    {
        AddIfConfidential(15, nameof(Pollutant));
    }

    // Uses "ONE"-based position to better correlate with IAIP code
    protected void AddIfConfidential(int position, string parameter)
    {
        if (ConfidentialParametersCode[position - 1] == '1') ConfidentialParameters.Add(parameter);
    }

    #endregion
}
