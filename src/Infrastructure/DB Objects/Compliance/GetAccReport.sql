USE airbranch;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE air.GetAccReport
    @AirsNumber varchar(12),
    @Id         int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves an ACC report for a given facility and year.

Input Parameters:
    @AirsNumber - The facility ID
    @Id - The tracking number of the ACC

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2022-02-22  DWaldron            Initial version
2022-03-09  DWaldron            Use Date Complete for memo date (#49)
2024-09-26  DWaldron            Fix City display

*******************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select c.STRTRACKINGNUMBER                                            as Id,
           convert(date, m.DATRECEIVEDDATE)                               as DateReceived,
           convert(date, m.DATCOMPLETEDATE)                               as DateComplete,
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
           l.STRCOUNTYNAME                                                as County,
           'FacilityAddress'       as Id,
           dbo.NullIfNaOrEmpty(f.STRFACILITYSTREET1)
                                   as Street,
           dbo.NullIfNaOrEmpty(f.STRFACILITYSTREET2)
                                   as Street2,
           trim(f.STRFACILITYCITY) as City,
           f.STRFACILITYSTATE      as State,
           f.STRFACILITYZIPCODE    as PostalCode,
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
      and c.STRTRACKINGNUMBER = @Id
      and f.STRAIRSNUMBER = @AirsNumber

    declare @params nvarchar(max) = concat_ws(':', '@AirsNumber', @AirsNumber, '@Id', @Id);
    exec air.LogReport 'ACC', @params;

    return 0;
END;

GO
