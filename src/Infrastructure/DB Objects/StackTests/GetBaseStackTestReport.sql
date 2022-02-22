USE airbranch;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE air.GetBaseStackTestReport
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves common information for a given stack test.

Input Parameters:
    @ReferenceNumber - The stack test reference number

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2022-02-22  DWaldron            Initial version

*******************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select convert(int, r.STRREFERENCENUMBER) as ReferenceNumber,
           lp.STRPOLLUTANTDESCRIPTION         as Pollutant,
           trim(r.STREMISSIONSOURCE)          as Source,
           convert(int, r.STRREPORTTYPE)      as ReportType,
           convert(int, r.STRDOCUMENTTYPE)    as DocumentType,
           trim(r.STRAPPLICABLEREQUIREMENT)   as ApplicableRequirement,
           trim(char(13) + char(10) + ' ' from r.MMOCOMMENTAREA)
                                              as Comments,
           r.DATRECEIVEDDATE                  as DateReceivedByApb,
           r.STRCONFIDENTIALDATA              as ConfidentialParametersCode,
           s.STRCOMPLIANCESTATEMENT           as ReportStatement,

           i.STRAIRSNUMBER                    as Id,
           f.STRFACILITYNAME                  as Name,
           lc.STRCOUNTYNAME                   as County,

           'Address'                          as Id,
           f.STRFACILITYSTREET1               as Street,
           f.STRFACILITYSTREET2               as Street2,
           f.STRFACILITYCITY                  as City,
           f.STRFACILITYSTATE                 as State,
           f.STRFACILITYZIPCODE               as PostalCode,

           'ReviewedByStaff'                  as Id,
           pr.STRFIRSTNAME                    as GivenName,
           pr.STRLASTNAME                     as FamilyName,

           'ComplianceManager'                as Id,
           pc.STRFIRSTNAME                    as GivenName,
           pc.STRLASTNAME                     as FamilyName,

           'TestingUnitManager'               as Id,
           pt.STRFIRSTNAME                    as GivenName,
           pt.STRLASTNAME                     as FamilyName,

           'TestDates'                        as Id,
           r.DATTESTDATESTART                 as StartDate,
           r.DATTESTDATEEND                   as EndDate
    from ISMPREPORTINFORMATION r
        left join LOOKUPPOLLUTANTS lp
        on r.STRPOLLUTANT = lp.STRPOLLUTANTCODE
        left join LOOKUPISMPCOMPLIANCESTATUS s
        on s.STRCOMPLIANCEKEY = r.STRCOMPLIANCESTATUS

        left join EPDUSERPROFILES pr
        on pr.NUMUSERID = r.STRREVIEWINGENGINEER
        left join EPDUSERPROFILES pc
        on pc.NUMUSERID = r.STRCOMPLIANCEMANAGER
        left join EPDUSERPROFILES pt
        on pt.NUMUSERID = convert(int, r.NUMREVIEWINGMANAGER)

        inner join ISMPMASTER i
        on i.STRREFERENCENUMBER = r.STRREFERENCENUMBER
        inner join APBFACILITYINFORMATION f
        on f.STRAIRSNUMBER = i.STRAIRSNUMBER
        left join dbo.LOOKUPCOUNTYINFORMATION lc
        on substring(f.STRAIRSNUMBER, 5, 3) = lc.STRCOUNTYCODE
    where STRDOCUMENTTYPE <> '001'
      and r.STRREFERENCENUMBER = @ReferenceNumber;

    select w.WitnessId     as Id,
           pw.STRFIRSTNAME as GivenName,
           pw.STRLASTNAME  as FamilyName
    from ISMPREPORTINFORMATION r
        inner join(select STRREFERENCENUMBER,
                          convert(int, STRWITNESSINGENGINEER)
                              as WitnessId
                   from ISMPREPORTINFORMATION
                   where STRWITNESSINGENGINEER <> 0
                   union
                   select STRREFERENCENUMBER,
                          convert(int, STRWITNESSINGENGINEER2)
                   from ISMPREPORTINFORMATION
                   where STRWITNESSINGENGINEER2 <> 0
                   union
                   select STRREFERENCENUMBER,
                          convert(int, STRWITNESSINGENGINEER)
                   from ISMPWITNESSINGENG
                   where STRWITNESSINGENGINEER <> 0) w
        on r.STRREFERENCENUMBER = w.STRREFERENCENUMBER
        left join EPDUSERPROFILES pw
        on pw.NUMUSERID = w.WitnessId
    where r.STRREFERENCENUMBER = @ReferenceNumber;
    
    return 0;
END;

GO