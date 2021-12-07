using Dapper;
using Domain.Facilities.Models;
using Domain.Facilities.Repositories;
using System.Data;

namespace Infrastructure.Facilities;

public class FacilitiesRepository : IFacilitiesRepository
{
    private readonly IDbConnection db;

    public FacilitiesRepository(IDbConnection connection) => db = connection;

    public Task<bool> FacilityExistsAsync(ApbFacilityId facilityId)
    {
        var query = @"SELECT CONVERT(bit, COUNT(*))
            FROM dbo.AFSFACILITYDATA
            where STRAIRSNUMBER = @AirsNumber
              and STRUPDATESTATUS <> 'H'";

        return db.ExecuteScalarAsync<bool>(query,
            new { AirsNumber = facilityId.DbFormattedString });
    }

    public async Task<Facility?> GetFacilityAsync(ApbFacilityId facilityId)
    {
        var query = @"SELECT f.STRAIRSNUMBER    as [Id],
                   f.STRFACILITYNAME  as [Name],
                   f.STRFACILITYCITY  as [City],
                   f.STRFACILITYSTATE as [State],
                   l.STRCOUNTYNAME    as [County]
            FROM dbo.APBFACILITYINFORMATION AS f
                inner JOIN dbo.AFSFACILITYDATA AS a
                ON f.STRAIRSNUMBER = a.STRAIRSNUMBER
                LEFT JOIN dbo.LOOKUPCOUNTYINFORMATION AS l
                ON substring(f.STRAIRSNUMBER, 5, 3) = l.STRCOUNTYCODE
            where f.STRAIRSNUMBER = @AirsNumber
              and a.STRUPDATESTATUS <> 'H'";

        var result = await db.QuerySingleOrDefaultAsync<Facility>(query,
            new { AirsNumber = facilityId.DbFormattedString });

        return result == default ? null : result;
    }
}
