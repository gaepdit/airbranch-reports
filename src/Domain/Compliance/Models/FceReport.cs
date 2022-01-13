﻿using Domain.Compliance.Models.WorkItems;
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

    public DateRange SupportingDataDateRange { get; set; }

    // Supporting compliance data

    public List<Inspection> Inspections { get; init; } = new List<Inspection>();
    public List<RmpInspection> RmpInspections { get; init; } = new List<RmpInspection>();
    public List<Acc> Accs { get; init; } = new List<Acc>();
    public List<Report> Reports { get; init; } = new List<Report>();
    public List<Notification> Notifications { get; init; } = new List<Notification>();
    public List<StackTest> StackTests { get; init; } = new List<StackTest>();
    public List<FeeYear> FeesHistory { get; init; } = new List<FeeYear>();
    public List<Enforcement> EnforcementHistory { get; init; } = new List<Enforcement>();
}