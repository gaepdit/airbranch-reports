using System.ComponentModel.DataAnnotations;

namespace Domain.Facilities.Models;

public record Facility
{
    // Facility identity

    [Display(Name = "AIRS Number")]
    public FacilityId? Id { get; init; }

    [Display(Name = "Company name")]
    public string Name { get; init; } = "";

    [Display(Name = "Facility description")]
    public string Description { get; init; } = "";

    // Location

    [Display(Name = "Company address")]
    public Address FacilityAddress { get; set; }

    [Display(Name = "County")]
    public string County { get; init; } = "";

    [Display(Name = "Geographic coordinates")]
    public GeoCoordinates? GeoCoordinates { get; set; }

    // Regulatory data

    public FacilityHeaderData? HeaderData { get; set; }
}
