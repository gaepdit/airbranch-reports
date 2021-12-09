using System.ComponentModel.DataAnnotations;

namespace Domain.Facilities.Models;

public record struct Facility
{
    [Display(Name = "AIRS Number")]
    public ApbFacilityId Id { get; init; }

    [Display(Name = "Company Name")]
    public string Name { get; init; }

    [Display(Name = "Company Location")]
    public string City { get; init; }
    public string County { get; init; }
    public string State { get; init; }
}
