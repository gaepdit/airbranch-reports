﻿namespace Domain.Organization.Models;

public record OrganizationInfo
{
    public string NameOfDirector { get; init; } = string.Empty;

    public static string Org => "Air Protection Branch";

    public static Address OrgAddress => new()
    {
        Street = "4244 International Parkway, Suite 120",
        City = "Atlanta",
        State = "Georgia",
        PostalCode = "30354",
    };

    public static string OrgPhoneNumber => "404-363-7000";
}
