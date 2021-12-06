namespace Domain.Organization.Models;

public record struct OrganizationInfo
{
    public string NameOfDirector { get; init; }
    public string Org { get; init; }
    public Address OrgAddress { get; init; }
    public string OrgPhoneNumber { get; init; }
}
