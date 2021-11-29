using Domain.Compliance.Models;
using Domain.Organization.Models;

namespace Domain.Compliance.Reports;

public record struct AccMemo
{
    public OrganizationInfo OrganizationInfo { get; init; }
    public Acc Acc { get; init; }
}
