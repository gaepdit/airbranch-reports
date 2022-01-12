using System.ComponentModel.DataAnnotations;

namespace Domain.Facilities.Models;

public record class Facility
{
    // Facility identity

    [Display(Name = "AIRS Number")]
    public ApbFacilityId? Id { get; init; }

    [Display(Name = "Facility name")]
    public string Name { get; init; } = "";

    [Display(Name = "Facility description")]
    public string Description { get; init; } = "";

    // Location

    [Display(Name = "Facility address")]
    public Address FacilityAddress { get; set; }

    [Display(Name = "County")]
    public string County { get; init; } = "";

    [Display(Name = "Geographic coordinates")]
    public GeoCoordinates? GeoCoordinates { get; set; }

    // Regulatory data

    public FacilityHeaderData? HeaderData { get; set; }
}
