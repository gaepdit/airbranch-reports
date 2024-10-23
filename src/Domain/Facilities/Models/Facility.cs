using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Facilities.Models;

public record Facility
{
    // Facility identity

    [JsonIgnore]
    [Display(Name = "AIRS Number")]
    public FacilityId? Id { get; init; }

    public string? FacilityId => Id?.FormattedId;

    // Description

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

    public RegulatoryData? RegulatoryData { get; set; }
}
