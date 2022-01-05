using System.ComponentModel.DataAnnotations;

namespace Domain.Compliance.Models;

public record class FceReport
{
    [Display(Name = "FCE tracking number")]
    public int Id { get; init; }

    public Facility? Facility { get; set; }

    [Display(Name = "FCE year")]
    public int FceYear { get; init; }

    [Display(Name = "Reviewed by")]
    public PersonName StaffReviewedBy { get; set; }

    [Display(Name = "Date completed")]
    public DateTime DateCompleted { get; init; }

    [Display(Name = "On-site inspection conducted")]
    public bool WithOnsiteInspection { get; init; }

    public string Comments { get; init; } = "";

    public DateRange SupportingDataDateRange { get; init; }

    // Supporting compliance data

}
