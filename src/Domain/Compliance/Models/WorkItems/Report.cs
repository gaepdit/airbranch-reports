﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Compliance.Models.WorkItems;

public record class Report
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Report period")]
    public string ReportPeriod { get; init; } = "";

    public DateRange ReportPeriodDates { get; init; }

    [Display(Name = "Date received")]
    public DateTime ReceivedDate { get; init; }

    public PersonName Reviewer { get; init; }

    [Display(Name = "Deviations reported")]
    public bool DeviationsReported { get; init; }

    public string Comments { get; init; } = "";
}
