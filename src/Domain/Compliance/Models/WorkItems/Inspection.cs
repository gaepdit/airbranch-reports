using System.ComponentModel.DataAnnotations;

namespace Domain.Compliance.Models.WorkItems;

public record class Inspection
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    public PersonName Inspector { get; set; }

    [Display(Name = "Date")]
    public DateRange InspectionDate { get; set; }

    [Display(Name = "Reason for inspection")]
    public string Reason { get; set; } = "";

    [Display(Name = "Operating")]
    public bool FacilityWasOperating { get; set; }

    [Display(Name = "Compliance status")]
    public string ComplianceStatus { get; set; } = "";
}
