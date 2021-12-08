namespace Domain.ValueObjects;

public record struct DateTimeRange
(
    DateTime StartDate,
    DateTime? EndDate
);
