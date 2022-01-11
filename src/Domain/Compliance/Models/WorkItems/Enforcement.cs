using System.ComponentModel.DataAnnotations;

namespace Domain.Compliance.Models.WorkItems;

public record class Enforcement
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Staff responsible")]
    public PersonName StaffResponsible { get; init; }

    [Display(Name = "Date")]
    public DateTime DiscoveryDate { get; init; }

    public string Type { get; init; } = "";
}
