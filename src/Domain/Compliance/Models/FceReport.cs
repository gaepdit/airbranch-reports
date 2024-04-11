using Domain.Compliance.Models.WorkItems;
using System.ComponentModel.DataAnnotations;

namespace Domain.Compliance.Models;

public record FceReport
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

    public DateRange SupportingDataDateRange { get; set; }

    // Supporting compliance data

    public List<Inspection> Inspections { get; init; } = [];
    public List<Inspection> RmpInspections { get; init; } = [];
    public List<Acc> Accs { get; init; } = [];
    public List<Report> Reports { get; init; } = [];
    public List<Notification> Notifications { get; init; } = [];
    public List<StackTestWork> StackTests { get; init; } = [];
    public List<FeeYear> FeesHistory { get; init; } = [];
    public List<Enforcement> EnforcementHistory { get; init; } = [];
}
