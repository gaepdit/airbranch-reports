namespace Domain.ValueObjects;

public record struct DateRange
(
    DateTime StartDate,
    DateTime? EndDate
);
