using Dapper;
using Domain.Compliance.Models;
using Domain.Compliance.Repositories;
using Domain.Facilities.Models;
using Domain.ValueObjects;
using System.Data;

namespace Infrastructure.Compliance;

public class ComplianceRepository : IComplianceRepository
{
    private readonly IDbConnection db;
    public ComplianceRepository(IDbConnection conn) => db = conn;

    // ACC
    public Task<bool> AccReportExistsAsync(ApbFacilityId facilityId, int year)
    {
        var query = @"select convert(bit, count(*))
            from dbo.SSCPITEMMASTER m
                inner join dbo.SSCPACCS c
                on m.STRTRACKINGNUMBER = c.STRTRACKINGNUMBER
            where STRAIRSNUMBER = @AirsNumber
              and year(DATACCREPORTINGYEAR) = @Year";

        return db.ExecuteScalarAsync<bool>(query, new
        {
            AirsNumber = facilityId.DbFormattedString,
            Year = year,
        });
    }

    public async Task<AccReport?> GetAccReportAsync(ApbFacilityId facilityId, int year)
    {
        if (!await AccReportExistsAsync(facilityId, year)) return null;

        var query = @"select c.STRTRACKINGNUMBER                                            as Id,
                   convert(date, m.DATRECEIVEDDATE)                               as DateReceived,
                   convert(date, m.DATCOMPLETEDATE)                               as DateComplete,
                   convert(date, m.DATACKNOLEDGMENTLETTERSENT)                    as DateAcknowledgmentLetterSent,
                   c.STRCOMMENTS                                                  as Comments,
                   year(c.DATACCREPORTINGYEAR)                                    as AccReportingYear,
                   convert(date, c.DATPOSTMARKDATE)                               as DatePostmarked,
                   convert(bit, IIF(c.STRPOSTMARKEDONTIME = 'True', 1, 0))        as PostmarkedByDeadline,
                   convert(bit, IIF(c.STRSIGNEDBYRO = 'True', 1, 0))              as SignedByResponsibleOfficial,
                   convert(bit, IIF(c.STRCORRECTACCFORMS = 'True', 1, 0))         as CorrectFormsUsed,
                   convert(bit, IIF(c.STRTITLEVCONDITIONSLISTED = 'True', 1, 0))  as AllTitleVConditionsListed,
                   convert(bit, IIF(c.STRACCCORRECTLYFILLEDOUT = 'True', 1, 0))   as CorrectlyFilledOut,
                   convert(bit, IIF(c.STRREPORTEDDEVIATIONS = 'True', 1, 0))      as DeviationsReported,
                   convert(bit, IIF(c.STRDEVIATIONSUNREPORTED = 'True', 1, 0))    as UnreportedDeviationsReported,
                   convert(bit, IIF(c.STRENFORCEMENTNEEDED = 'True', 1, 0))       as EnforcementRecommended,
                   convert(bit, IIF(c.STRKNOWNDEVIATIONSREPORTED = 'True', 1, 0)) as AllDeviationsReported,
                   convert(bit, IIF(c.STRRESUBMITTALREQUIRED = 'True', 1, 0))     as ResubmittalRequested,
                   f.STRAIRSNUMBER                                                as Id,
                   f.STRFACILITYNAME                                              as Name,
                   f.STRFACILITYCITY                                              as City,
                   f.STRFACILITYSTATE                                             as State,
                   l.STRCOUNTYNAME                                                as County,
                   convert(int, m.STRRESPONSIBLESTAFF)                            as Id,
                   p.STRFIRSTNAME                                                 as GivenName,
                   p.STRLASTNAME                                                  as FamilyName
            from dbo.SSCPITEMMASTER AS m
                inner join dbo.SSCPACCS AS c
                ON c.STRTRACKINGNUMBER = m.STRTRACKINGNUMBER
                left join dbo.EPDUSERPROFILES AS p
                ON m.STRRESPONSIBLESTAFF = p.NUMUSERID
                inner join dbo.APBFACILITYINFORMATION AS f
                on m.STRAIRSNUMBER = f.STRAIRSNUMBER
                left join dbo.LOOKUPCOUNTYINFORMATION AS l
                ON substring(f.STRAIRSNUMBER, 5, 3) = l.STRCOUNTYCODE
            where m.STRDELETE is null
              and year(c.DATACCREPORTINGYEAR) = @Year
              and f.STRAIRSNUMBER = @AirsNumber";

        var param = new
        {
            AirsNumber = facilityId.DbFormattedString,
            Year = year,
        };

        return (await db.QueryAsync<AccReport, Facility, PersonName, AccReport>(
            query,
            (a, f, n) =>
            {
                a.Facility = f;
                a.StaffResponsible = n;
                return a;
            },
            param)).Single();
    }

    // FCE
    public Task<bool> FceReportExistsAsync(ApbFacilityId facilityId, int year)
    {
        throw new NotImplementedException();
    }

    public Task<FceReport?> GetFceReportAsync(ApbFacilityId facilityId, int year)
    {
        throw new NotImplementedException();
    }
}
