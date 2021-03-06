using Dapper;
using Domain.Organization.Models;
using Domain.Organization.Repositories;
using Domain.ValueObjects;
using System.Data;

namespace Infrastructure.Organization;

public class OrganizationRepository : IOrganizationRepository
{
    // ReSharper disable once InconsistentNaming
    private readonly IDbConnection db;
    public OrganizationRepository(IDbConnection conn) => db = conn;

    public async Task<OrganizationInfo> GetAsync()
    {
        var director = await db.ExecuteScalarAsync<string>("air.GetManagement",
            new { Type = "EpdDirector" },
            commandType: CommandType.StoredProcedure);

        var organizationInfo = new OrganizationInfo
        {
            NameOfDirector = director,
            Org = "Air Protection Branch",
            OrgAddress = new Address
            {
                Street = "4244 International Parkway, Suite 120",
                Street2 = null,
                City = "Atlanta",
                State = "Georgia",
                PostalCode = "30354",
            },
            OrgPhoneNumber = "404-363-7000",
        };

        return organizationInfo;
    }
}
