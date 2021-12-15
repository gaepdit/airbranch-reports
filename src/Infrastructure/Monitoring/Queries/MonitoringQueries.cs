namespace Infrastructure.Monitoring.Queries;

internal static class MonitoringQueries
{
    public static string StackTestReportExists = @"
select convert(bit, count(1))
from dbo.ISMPMASTER m
    inner join ISMPREPORTINFORMATION r
    on m.STRREFERENCENUMBER = r.STRREFERENCENUMBER
where r.STRDOCUMENTTYPE <> '001'
  and m.STRAIRSNUMBER = @AirsNumber
  and convert(int, m.STRREFERENCENUMBER) = @ReferenceNumber
";

    public static string StackTestReport = @"
select convert(int, r.STRREFERENCENUMBER) as ReferenceNumber,
       lp.STRPOLLUTANTDESCRIPTION         as Pollutant,
       trim(r.STREMISSIONSOURCE)          as Source,
       convert(int, r.STRREPORTTYPE)      as ReportType,
       convert(int, r.STRDOCUMENTTYPE)    as DocumentType,
       trim(r.STRAPPLICABLEREQUIREMENT)   as ApplicableRequirement,
       trim(r.MMOCOMMENTAREA)             as Comments,
       r.DATRECEIVEDDATE                  as DateReceivedByApb,
       r.STRCONFIDENTIALDATA              as ConfidentialParametersCode,

       i.STRAIRSNUMBER                    as Id,
       f.STRFACILITYNAME                  as Name,
       f.STRFACILITYCITY                  as City,
       f.STRFACILITYSTATE                 as [State],
       lc.STRCOUNTYNAME                   as County,

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

select w.WitnessId                        as Id,
       pw.STRFIRSTNAME                    as GivenName,
       pw.STRLASTNAME                     as FamilyName
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
where r.STRREFERENCENUMBER = @ReferenceNumber;";

    public static string StackTestReportOneStack = @"
select trim(r.STRCONTROLEQUIPMENTDATA)      as ControlEquipmentInfo,
       trim(d.STRPERCENTALLOWABLE)          as PercentAllowable,
       'MaxOperatingCapacity'               as Id,
       trim(d.STRMAXOPERATINGCAPACITY)      as Value,
       u1.STRUNITDESCRIPTION                as Units,
       'OperatingCapacity'                  as Id,
       trim(d.STROPERATINGCAPACITY)         as Value,
       u2.STRUNITDESCRIPTION                as Units,
       'AvgPollutantConcentration'          as Id,
       trim(d.STRPOLLUTANTCONCENTRATIONAVG) as Value,
       u3.STRUNITDESCRIPTION                as Units,
       'AvgEmissionRate'                    as Id,
       trim(d.STREMISSIONRATEAVG)           as Value,
       u4.STRUNITDESCRIPTION                as Units
from ISMPREPORTINFORMATION r
    inner join ISMPREPORTONESTACK d
    on r.STRREFERENCENUMBER = d.STRREFERENCENUMBER
    left join LOOKUPUNITS u1
    on u1.STRUNITKEY = d.STRMAXOPERATINGCAPACITYUNIT
        and u1.STRUNITKEY <> '00000'
    left join LOOKUPUNITS u2
    on u2.STRUNITKEY = d.STROPERATINGCAPACITYUNIT
        and u2.STRUNITKEY <> '00000'
    left join LOOKUPUNITS u3
    on u3.STRUNITKEY = d.STRPOLLUTANTCONCENTRATIONUNIT
        and u3.STRUNITKEY <> '00000'
    left join LOOKUPUNITS u4
    on u4.STRUNITKEY = d.STREMISSIONRATEUNIT
        and u4.STRUNITKEY <> '00000'
where convert(int, r.STRREFERENCENUMBER) = @ReferenceNumber;

select trim(t.Value) as Value,
       u.STRUNITDESCRIPTION               as Units
from (
    select STRREFERENCENUMBER,
           1                             as Id,
           STRALLOWABLEEMISSIONRATE1     as Value,
           STRALLOWABLEEMISSIONRATEUNIT1 as UnitCode
    from ISMPREPORTONESTACK
    union
    select STRREFERENCENUMBER,
           2 as Id,
           STRALLOWABLEEMISSIONRATE2,
           STRALLOWABLEEMISSIONRATEUNIT2
    from ISMPREPORTONESTACK
    union
    select STRREFERENCENUMBER,
           3 as Id,
           STRALLOWABLEEMISSIONRATE3,
           STRALLOWABLEEMISSIONRATEUNIT3
    from ISMPREPORTONESTACK) t
    inner join LOOKUPUNITS u
    on u.STRUNITKEY = t.UnitCode
        and u.STRUNITKEY <> '00000'
where convert(int, t.STRREFERENCENUMBER) = @ReferenceNumber
order by t.Id;

select trim(RunNumber)              as RunNumber,
       trim(GasTemperature)         as GasTemperature,
       trim(GasMoisture)            as GasMoisture,
       trim(GasFlowRateAscfm)       as GasFlowRateAscfm,
       trim(GasFlowRateDscfm)       as GasFlowRateDscfm,
       trim(PollutantConcentration) as PollutantConcentration,
       trim(EmissionRate)           as EmissionRate,
       isnull(ConfidentialParametersCode, '') as ConfidentialParametersCode
from (
    select 1                                       as Id,
           s.STRREFERENCENUMBER,
           s.STRRUNNUMBER1A                        as RunNumber,
           s.STRGASTEMPERATURE1A                   as GasTemperature,
           s.STRGASMOISTURE1A                      as GasMoisture,
           s.STRGASFLOWRATEACFM1A                  as GasFlowRateAscfm,
           s.STRGASFLOWRATEDSCFM1A                 as GasFlowRateDscfm,
           s.STRPOLLUTANTCONCENTRATION1A           as PollutantConcentration,
           s.STREMISSIONRATE1A                     as EmissionRate,
           substring(r.STRCONFIDENTIALDATA, 35, 7) as ConfidentialParametersCode
    from ISMPREPORTONESTACK s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select 2,
           s.STRREFERENCENUMBER,
           s.STRRUNNUMBER1B,
           s.STRGASTEMPERATURE1B,
           s.STRGASMOISTURE1B,
           s.STRGASFLOWRATEACFM1B,
           s.STRGASFLOWRATEDSCFM1B,
           s.STRPOLLUTANTCONCENTRATION1B,
           s.STREMISSIONRATE1B,
           substring(r.STRCONFIDENTIALDATA, 42, 7)
    from ISMPREPORTONESTACK s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select 3,
           IIF(r.STRDOCUMENTTYPE in ('003', '004'), s.STRREFERENCENUMBER, null),
           s.STRRUNNUMBER1C,
           s.STRGASTEMPERATURE1C,
           s.STRGASMOISTURE1C,
           s.STRGASFLOWRATEACFM1C,
           s.STRGASFLOWRATEDSCFM1C,
           s.STRPOLLUTANTCONCENTRATION1C,
           s.STREMISSIONRATE1C,
           IIF(r.STRDOCUMENTTYPE in ('003', '004'), substring(r.STRCONFIDENTIALDATA, 49, 7), null)
    from ISMPREPORTONESTACK s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select 4,
           IIF(r.STRDOCUMENTTYPE in ('004'), s.STRREFERENCENUMBER, null),
           s.STRRUNNUMBER1D,
           s.STRGASTEMPERATURE1D,
           s.STRGASMOISTURE1D,
           s.STRGASFLOWRATEACFM1D,
           s.STRGASFLOWRATEDSCFM1D,
           s.STRPOLLUTANTCONCENTRATION1D,
           s.STREMISSIONRATE1D,
           IIF(r.STRDOCUMENTTYPE in ('003', '004'), substring(r.STRCONFIDENTIALDATA, 56, 7), null)
    from ISMPREPORTONESTACK s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
) t
where convert(int, STRREFERENCENUMBER) = @ReferenceNumber
order by Id;
";

}
