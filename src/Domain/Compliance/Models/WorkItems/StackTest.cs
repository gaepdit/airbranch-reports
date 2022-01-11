using System.ComponentModel.DataAnnotations;

namespace Domain.Compliance.Models.WorkItems;

public record class StackTest
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Test Ref #")]
    public int ReferenceNumber { get; init; }

    [Display(Name = "Date received")]
    public DateTime ReceivedDate { get; init; }

    public PersonName Reviewer { get; init; }

    [Display(Name = "Compliance status")]
    public string ComplianceStatus { get; init; } = "";

    [Display(Name = "Source tested")]
    public string SourceTested { get; init; } = "";
}
