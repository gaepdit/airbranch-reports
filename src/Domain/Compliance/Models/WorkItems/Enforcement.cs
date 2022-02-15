using System.ComponentModel.DataAnnotations;

namespace Domain.Compliance.Models.WorkItems;

public record Enforcement
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Staff responsible")]
    public PersonName StaffResponsible { get; set; }

    [Display(Name = "Date")]
    public DateTime EnforcementDate { get; init; }

    [Display(Name = "Type")]
    public string EnforcementType { get; init; } = "";
}
