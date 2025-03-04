﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Compliance.Models.WorkItems;

public record Notification
{
    [Display(Name = "Tracking #")]
    public int Id { get; init; }

    [Display(Name = "Date received")]
    public DateTime ReceivedDate { get; init; }

    public PersonName Reviewer { get; set; }

    [Display(Name = "Notification type")]
    public string Type { get; init; } = "";

    public string Comments { get; init; } = "";
}
