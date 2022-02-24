﻿USE airbranch;
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
2022-02-24  DWaldron            Exclude deleted stack tests

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
    from dbo.ISMPREPORTINFORMATION r
        left join dbo.LOOKUPPOLLUTANTS lp
        on r.STRPOLLUTANT = lp.STRPOLLUTANTCODE
        left join dbo.LOOKUPISMPCOMPLIANCESTATUS s
        on s.STRCOMPLIANCEKEY = r.STRCOMPLIANCESTATUS

        left join dbo.EPDUSERPROFILES pr
        on pr.NUMUSERID = r.STRREVIEWINGENGINEER
        left join dbo.EPDUSERPROFILES pc
        on pc.NUMUSERID = r.STRCOMPLIANCEMANAGER
        left join dbo.EPDUSERPROFILES pt
        on pt.NUMUSERID = convert(int, r.NUMREVIEWINGMANAGER)

        inner join dbo.ISMPMASTER i
        on i.STRREFERENCENUMBER = r.STRREFERENCENUMBER
        inner join dbo.APBFACILITYINFORMATION f
        on f.STRAIRSNUMBER = i.STRAIRSNUMBER
        left join dbo.LOOKUPCOUNTYINFORMATION lc
        on substring(f.STRAIRSNUMBER, 5, 3) = lc.STRCOUNTYCODE
    where r.STRDOCUMENTTYPE <> '001'
      and r.STRDELETE is null
      and r.STRREFERENCENUMBER = @ReferenceNumber;

    select w.WitnessId     as Id,
           p.STRFIRSTNAME as GivenName,
           p.STRLASTNAME  as FamilyName
    from dbo.ISMPREPORTINFORMATION r
        inner join(select STRREFERENCENUMBER,
                          convert(int, STRWITNESSINGENGINEER)
                              as WitnessId
                   from dbo.ISMPREPORTINFORMATION
                   where STRWITNESSINGENGINEER <> 0
                   union
                   select STRREFERENCENUMBER,
                          convert(int, STRWITNESSINGENGINEER2)
                   from dbo.ISMPREPORTINFORMATION
                   where STRWITNESSINGENGINEER2 <> 0
                   union
                   select STRREFERENCENUMBER,
                          convert(int, STRWITNESSINGENGINEER)
                   from dbo.ISMPWITNESSINGENG
                   where STRWITNESSINGENGINEER <> 0) w
        on r.STRREFERENCENUMBER = w.STRREFERENCENUMBER
        left join dbo.EPDUSERPROFILES p
        on p.NUMUSERID = w.WitnessId
    where r.STRDOCUMENTTYPE <> '001'
      and r.STRDELETE is null
      and r.STRREFERENCENUMBER = @ReferenceNumber;

    return 0;
END;

GO