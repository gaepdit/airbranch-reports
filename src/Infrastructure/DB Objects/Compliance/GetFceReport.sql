﻿USE airbranch;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE air.GetFceReport
    @AirsNumber            varchar(12),
    @Id                    int,
    @FceDataPeriod         int,
    @FceExtendedDataPeriod int
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   
  Retrieves an FCE report for a given facility and FCE ID.

Input Parameters:
  @AirsNumber - The facility ID
  @Id - The tracking number of the FCE
  @FceDataPeriod - The number of years covered by the FCE
  @FceExtendedDataPeriod - The number of years of additional data retrieved
    (fees history and enforcement history)

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2022-02-22  DWaldron            Initial version
2023-09-28  DWaldron            Ensure date ranges are inclusive (airbranch-reports#92)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    declare @FalseString varchar(5) = 'false';

    select f.STRFCENUMBER                   as Id,
           f.STRFCEYEAR                     as FceYear,
           convert(date, f.DATFCECOMPLETED) as DateCompleted,
           convert(bit, IIF(f.STRSITEINSPECTION = @FalseString, 0, 1))
                                            as WithOnsiteInspection,
           f.STRFCECOMMENTS                 as Comments,
           f.STRREVIEWER                    as Id, -- StaffReviewedBy
           p.STRFIRSTNAME                   as GivenName,
           p.STRLASTNAME                    as FamilyName,
           'SupportingDataDateRange'        as Id,
           dateadd(year, -@FceDataPeriod, convert(date, f.DATFCECOMPLETED))
                                            as StartDate,
           convert(date, f.DATFCECOMPLETED) as EndDate
    from dbo.SSCPFCE f
        inner join dbo.EPDUSERPROFILES p
        on f.STRREVIEWER = p.NUMUSERID
    where f.STRFCENUMBER = @Id;

-- Inspections
    select m.STRTRACKINGNUMBER                     as Id,
           i.STRINSPECTIONREASON                   as Reason,
           convert(bit, IIF(i.STRFACILITYOPERATING = @FalseString, 0, 1))
                                                   as FacilityWasOperating,
           i.STRINSPECTIONCOMPLIANCESTATUS         as ComplianceStatus,
           i.STRINSPECTIONCOMMENTS                 as Comments,
           m.STRRESPONSIBLESTAFF                   as Id, -- Inspector
           p.STRFIRSTNAME                          as GivenName,
           p.STRLASTNAME                           as FamilyName,
           'InspectionDate'                        as Id,
           convert(date, i.DATINSPECTIONDATESTART) as StartDate,
           convert(date, i.DATINSPECTIONDATEEND)   as EndDate
    from dbo.SSCPINSPECTIONS as i
        inner join dbo.SSCPITEMMASTER as m
        on m.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
        inner join dbo.SSCPFCE f
        on f.STRFCENUMBER = @Id
            and convert(date, i.DATINSPECTIONDATESTART) between
               dateadd(year, -@FceDataPeriod, convert(date, f.DATFCECOMPLETED))
               and convert(date, f.DATFCECOMPLETED)
        left join dbo.EPDUSERPROFILES as p
        on m.STRRESPONSIBLESTAFF = p.NUMUSERID
    where m.STREVENTTYPE = '02'
      and m.STRAIRSNUMBER = @AirsNumber
      and m.STRDELETE is null
    order by i.DATINSPECTIONDATESTART desc;

-- RmpInspections
    select m.STRTRACKINGNUMBER                     as Id,
           i.STRINSPECTIONREASON                   as Reason,
           convert(bit, IIF(i.STRFACILITYOPERATING = @FalseString, 0, 1))
                                                   as FacilityWasOperating,
           i.STRINSPECTIONCOMPLIANCESTATUS         as ComplianceStatus,
           i.STRINSPECTIONCOMMENTS                 as Comments,
           m.STRRESPONSIBLESTAFF                   as Id, -- Inspector
           p.STRFIRSTNAME                          as GivenName,
           p.STRLASTNAME                           as FamilyName,
           'InspectionDate'                        as Id,
           convert(date, i.DATINSPECTIONDATESTART) as StartDate,
           convert(date, i.DATINSPECTIONDATEEND)   as EndDate
    from dbo.SSCPINSPECTIONS as i
        inner join dbo.SSCPITEMMASTER as m
        on m.STRTRACKINGNUMBER = i.STRTRACKINGNUMBER
        inner join dbo.SSCPFCE f
        on f.STRFCENUMBER = @Id
            and convert(date, i.DATINSPECTIONDATESTART) between
               dateadd(year, -@FceDataPeriod, convert(date, f.DATFCECOMPLETED))
               and convert(date, f.DATFCECOMPLETED)
        left join dbo.EPDUSERPROFILES as p
        on m.STRRESPONSIBLESTAFF = p.NUMUSERID
    where m.STREVENTTYPE = '07'
      and m.STRAIRSNUMBER = @AirsNumber
      and m.STRDELETE is null
    order by i.DATINSPECTIONDATESTART desc;

-- ACCs
    select m.STRTRACKINGNUMBER         as Id,
           year(a.DATACCREPORTINGYEAR) as AccReportingYear,
           m.DATRECEIVEDDATE           as ReceivedDate,
           a.STRREPORTEDDEVIATIONS     as DeviationsReported,
           m.STRRESPONSIBLESTAFF       as Id, -- Reviewer
           p.STRFIRSTNAME              as GivenName,
           p.STRLASTNAME               as FamilyName
    from dbo.SSCPITEMMASTER as m
        left join dbo.EPDUSERPROFILES as p
        on m.STRRESPONSIBLESTAFF = p.NUMUSERID
        inner join dbo.SSCPACCS as a
        on a.STRTRACKINGNUMBER = m.STRTRACKINGNUMBER
        inner join dbo.SSCPFCE f
        on f.STRFCENUMBER = @Id
            and convert(date, m.DATRECEIVEDDATE) between
               dateadd(year, -@FceDataPeriod, convert(date, f.DATFCECOMPLETED))
               and convert(date, f.DATFCECOMPLETED)
    where m.STREVENTTYPE = '04'
      and m.STRAIRSNUMBER = @AirsNumber
      and m.STRDELETE is null
    order by m.DATCOMPLETEDATE desc, m.STRTRACKINGNUMBER desc;

-- Reports
    select m.STRTRACKINGNUMBER                      as Id,
           r.STRREPORTPERIOD                        as ReportPeriod,
           m.DATRECEIVEDDATE                        as ReceivedDate,
           r.STRSHOWDEVIATION                       as DeviationsReported,
           r.STRGENERALCOMMENTS                     as Comments,
           m.STRRESPONSIBLESTAFF                    as Id, -- Reviewer
           p.STRFIRSTNAME                           as GivenName,
           p.STRLASTNAME                            as FamilyName,
           'ReportPeriodDateRange'                  as Id,
           convert(date, r.DATREPORTINGPERIODSTART) as StartDate,
           convert(date, r.DATREPORTINGPERIODEND)   as EndDate
    from dbo.SSCPITEMMASTER as m
        left join dbo.EPDUSERPROFILES as p
        on m.STRRESPONSIBLESTAFF = p.NUMUSERID
        inner join dbo.SSCPREPORTS AS r
        on r.STRTRACKINGNUMBER = m.STRTRACKINGNUMBER
        inner join dbo.SSCPFCE f
        on f.STRFCENUMBER = @Id
            and convert(date, m.DATRECEIVEDDATE) between
               dateadd(year, -@FceDataPeriod, convert(date, f.DATFCECOMPLETED))
               and convert(date, f.DATFCECOMPLETED)
    where m.STREVENTTYPE = '01'
      and m.STRAIRSNUMBER = @AirsNumber
      and m.STRDELETE is null
    order by m.DATCOMPLETEDATE desc, m.STRTRACKINGNUMBER desc;

-- Notifications
    select m.STRTRACKINGNUMBER        as Id,
           m.DATRECEIVEDDATE          as ReceivedDate,
           iif(n.STRNOTIFICATIONTYPE = '01', n.STRNOTIFICATIONTYPEOTHER,
               l.STRNOTIFICATIONDESC) as Type,
           n.STRNOTIFICATIONCOMMENT   as Comments,
           m.STRRESPONSIBLESTAFF      as Id, -- Reviewer
           p.STRFIRSTNAME             as GivenName,
           p.STRLASTNAME              as FamilyName
    from dbo.SSCPITEMMASTER as m
        left join dbo.EPDUSERPROFILES as p
        on m.STRRESPONSIBLESTAFF = p.NUMUSERID
        inner join dbo.SSCPNOTIFICATIONS AS n
        on n.STRTRACKINGNUMBER = m.STRTRACKINGNUMBER
        inner join dbo.SSCPFCE f
        on f.STRFCENUMBER = @Id
            and convert(date, m.DATRECEIVEDDATE) between
               dateadd(year, -@FceDataPeriod, convert(date, f.DATFCECOMPLETED))
               and convert(date, f.DATFCECOMPLETED)
        left join dbo.LOOKUPSSCPNOTIFICATIONS AS l
        on l.STRNOTIFICATIONKEY = n.STRNOTIFICATIONTYPE
    where m.STREVENTTYPE = '05'
      and m.STRAIRSNUMBER = @AirsNumber
      and m.STRDELETE is null
    order by m.DATCOMPLETEDATE desc, m.STRTRACKINGNUMBER desc;

-- StackTests
    select m.STRTRACKINGNUMBER     as Id,
           t.STRREFERENCENUMBER    as ReferenceNumber,
           m.DATRECEIVEDDATE       as ReceivedDate,
           lc.STRCOMPLIANCESTATUS  as ComplianceStatus,
           STRPOLLUTANTDESCRIPTION as PollutantMeasured,
           STREMISSIONSOURCE       as SourceTested,
           m.STRRESPONSIBLESTAFF   as Id, -- Reviewer
           p.STRFIRSTNAME          as GivenName,
           p.STRLASTNAME           as FamilyName
    from dbo.SSCPITEMMASTER as m
        left join dbo.EPDUSERPROFILES as p
        on m.STRRESPONSIBLESTAFF = p.NUMUSERID
        inner join dbo.SSCPTESTREPORTS as t
        on t.STRTRACKINGNUMBER = m.STRTRACKINGNUMBER
        inner join dbo.ISMPREPORTINFORMATION as i
        on i.STRREFERENCENUMBER = t.STRREFERENCENUMBER
        inner join dbo.SSCPFCE f
        on f.STRFCENUMBER = @Id
            and convert(date, m.DATRECEIVEDDATE) between
               dateadd(year, -@FceDataPeriod, convert(date, f.DATFCECOMPLETED))
               and convert(date, f.DATFCECOMPLETED)
        left join dbo.LOOKUPPOLLUTANTS as lp
        on lp.STRPOLLUTANTCODE = i.STRPOLLUTANT
        inner join dbo.LOOKUPISMPCOMPLIANCESTATUS as lc
        on lc.STRCOMPLIANCEKEY = i.STRCOMPLIANCESTATUS
    where m.STREVENTTYPE = '03'
      and m.STRAIRSNUMBER = @AirsNumber
      and m.STRDELETE is null
    order by m.DATCOMPLETEDATE desc, m.STRTRACKINGNUMBER desc;

-- FeesHistory
    select a.NUMFEEYEAR                        as Year,
           inv.InvoicedAmount,
           trx.AmountPaid,
           inv.InvoicedAmount - trx.AmountPaid as Balance,
           ls.STRIAIPDESC                      as Status
    from dbo.FS_ADMIN as a
        inner join dbo.SSCPFCE f
        on f.STRFCENUMBER = @Id
            and a.NUMFEEYEAR between
               (year(f.DATFCECOMPLETED) - @FceExtendedDataPeriod)
               and year(f.DATFCECOMPLETED)
        left join
    (select STRAIRSNUMBER,
            NUMFEEYEAR,
            sum(convert(decimal(10, 2), NUMAMOUNT)) as InvoicedAmount
     from dbo.FS_FEEINVOICE
     where ACTIVE = '1'
     group by STRAIRSNUMBER, NUMFEEYEAR) as inv
        on a.STRAIRSNUMBER = inv.STRAIRSNUMBER
            and a.NUMFEEYEAR = inv.NUMFEEYEAR
        left join
    (select STRAIRSNUMBER,
            NUMFEEYEAR,
            sum(convert(decimal(10, 2), NUMPAYMENT)) as AmountPaid
     from dbo.FS_TRANSACTIONS
     where ACTIVE = '1'
     group by STRAIRSNUMBER, NUMFEEYEAR) as trx
        on a.STRAIRSNUMBER = trx.STRAIRSNUMBER
            and a.NUMFEEYEAR = trx.NUMFEEYEAR
        left join dbo.FSLK_ADMIN_STATUS as ls
        on a.NUMCURRENTSTATUS = ls.ID
    where a.ACTIVE = '1'
      and a.STRAIRSNUMBER = @AirsNumber
    order by Year desc;

-- EnforcementHistory
    select e.STRENFORCEMENTNUMBER      as Id,
           convert(date, e.DATLONSENT) as EnforcementDate,
           'Letter of Noncompliance'   as EnforcementType,
           e.NUMSTAFFRESPONSIBLE       as Id, -- StaffResponsible
           p.STRFIRSTNAME              as GivenName,
           p.STRLASTNAME               as FamilyName
    from dbo.SSCP_AUDITEDENFORCEMENT e
        left join dbo.EPDUSERPROFILES as p
        on e.NUMSTAFFRESPONSIBLE = p.NUMUSERID
        inner join dbo.SSCPFCE f
        on f.STRFCENUMBER = @Id
            and convert(date, e.DATLONSENT) between
               dateadd(year, -@FceExtendedDataPeriod, f.DATFCECOMPLETED)
               and convert(date, f.DATFCECOMPLETED)
    where e.STRLONSENT = 'True'
      and e.STRAFSKEYACTIONNUMBER is null
      and (e.IsDeleted = 0 or e.IsDeleted is null)
      and e.STRAIRSNUMBER = @AirsNumber

    union
    select e.STRENFORCEMENTNUMBER      as Id,
           convert(date, e.DATNOVSENT) as EnforcementDate,
           'Notice of Violation'       as EnforcementType,
           e.NUMSTAFFRESPONSIBLE       as Id,
           p.STRFIRSTNAME              as GivenName,
           p.STRLASTNAME               as FamilyName
    from dbo.SSCP_AUDITEDENFORCEMENT e
        left join dbo.EPDUSERPROFILES as p
        on e.NUMSTAFFRESPONSIBLE = p.NUMUSERID
        inner join dbo.SSCPFCE f
        on f.STRFCENUMBER = @Id
            and convert(date, e.DATNOVSENT) between
               dateadd(year, -@FceExtendedDataPeriod, f.DATFCECOMPLETED)
               and convert(date, f.DATFCECOMPLETED)
    where e.STRNOVSENT = 'True'
      and e.STRAFSNOVSENTNUMBER is not null
      and e.STRAFSKEYACTIONNUMBER is not null
      and (e.IsDeleted = 0 or e.IsDeleted is null)
      and e.STRAIRSNUMBER = @AirsNumber

    union
    select e.STRENFORCEMENTNUMBER         as Id,
           convert(date, e.DATCOEXECUTED) as EnforcementDate,
           iif(e.STRCONUMBER is not null,
               concat('Consent Order #', e.STRCONUMBER),
               'Consent Order')           as EnforcementType,
           e.NUMSTAFFRESPONSIBLE          as Id,
           p.STRFIRSTNAME                 as GivenName,
           p.STRLASTNAME                  as FamilyName
    from dbo.SSCP_AUDITEDENFORCEMENT e
        left join dbo.EPDUSERPROFILES as p
        on e.NUMSTAFFRESPONSIBLE = p.NUMUSERID
        inner join dbo.SSCPFCE f
        on f.STRFCENUMBER = @Id
            and convert(date, e.DATCOEXECUTED) between
               dateadd(year, -@FceExtendedDataPeriod, f.DATFCECOMPLETED)
               and convert(date, f.DATFCECOMPLETED)
    where e.STRCOEXECUTED = 'True'
      and e.STRAFSCOEXECUTEDNUMBER is not null
      and e.STRAFSKEYACTIONNUMBER is not null
      and (e.IsDeleted = 0 or e.IsDeleted is null)
      and e.STRAIRSNUMBER = @AirsNumber

    union
    select e.STRENFORCEMENTNUMBER         as Id,
           convert(date, e.DATAOEXECUTED) as EnforcementDate,
           iif(e.STRCONUMBER is not null,
               concat('Administrative Order #', e.STRCONUMBER),
               'Administrative Order')    as EnforcementType,
           e.NUMSTAFFRESPONSIBLE          as Id,
           p.STRFIRSTNAME                 as GivenName,
           p.STRLASTNAME                  as FamilyName
    from dbo.SSCP_AUDITEDENFORCEMENT e
        left join dbo.EPDUSERPROFILES as p
        on e.NUMSTAFFRESPONSIBLE = p.NUMUSERID
        inner join dbo.SSCPFCE f
        on f.STRFCENUMBER = @Id
            and convert(date, e.DATAOEXECUTED) between
               dateadd(year, -@FceExtendedDataPeriod, f.DATFCECOMPLETED)
               and convert(date, f.DATFCECOMPLETED)
    where e.STRAOEXECUTED = 'True'
      and e.STRAFSAOTOAGNUMBER is not null
      and e.STRAFSKEYACTIONNUMBER is not null
      and (e.IsDeleted = 0 or e.IsDeleted is null)
      and e.STRAIRSNUMBER = @AirsNumber

    order by EnforcementDate desc, Id desc;

    declare @params nvarchar(max) = concat_ws(':', '@AirsNumber', @AirsNumber, '@Id', @Id);
    exec air.LogReport 'FCE', @params;

    return 0;
END;

GO
