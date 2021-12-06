namespace Domain.ValueObjects;

public record struct Address
(
    string Street,
    string? Street2,
    string City,
    string State,
    string PostalCode
);
