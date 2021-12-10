namespace Infrastructure.Monitoring.Queries;

internal static class MonitoringQueries
{

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
where r.STRREFERENCENUMBER = @ReferenceNumber;

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
where r.STRREFERENCENUMBER = @ReferenceNumber;

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
       trim(EmissionRate)           as EmissionRate
from (
    select STRREFERENCENUMBER,
           STRRUNNUMBER1A              as RunNumber,
           STRGASTEMPERATURE1A         as GasTemperature,
           STRGASMOISTURE1A            as GasMoisture,
           STRGASFLOWRATEACFM1A        as GasFlowRateAscfm,
           STRGASFLOWRATEDSCFM1A       as GasFlowRateDscfm,
           STRPOLLUTANTCONCENTRATION1A as PollutantConcentration,
           STREMISSIONRATE1A           as EmissionRate
    from ISMPREPORTONESTACK
    union
    select STRREFERENCENUMBER,
           STRRUNNUMBER1B,
           STRGASTEMPERATURE1B,
           STRGASMOISTURE1B,
           STRGASFLOWRATEACFM1B,
           STRGASFLOWRATEDSCFM1B,
           STRPOLLUTANTCONCENTRATION1B,
           STREMISSIONRATE1B
    from ISMPREPORTONESTACK
    union
    select STRREFERENCENUMBER,
           STRRUNNUMBER1C,
           STRGASTEMPERATURE1C,
           STRGASMOISTURE1C,
           STRGASFLOWRATEACFM1C,
           STRGASFLOWRATEDSCFM1C,
           STRPOLLUTANTCONCENTRATION1C,
           STREMISSIONRATE1C
    from ISMPREPORTONESTACK
    union
    select STRREFERENCENUMBER,
           STRRUNNUMBER1D,
           STRGASTEMPERATURE1D,
           STRGASMOISTURE1D,
           STRGASFLOWRATEACFM1D,
           STRGASFLOWRATEDSCFM1D,
           STRPOLLUTANTCONCENTRATION1D,
           STREMISSIONRATE1D
    from ISMPREPORTONESTACK) t
where t.EmissionRate not in ('', ' ', 'N/A')
  and convert(int, t.STRREFERENCENUMBER) = @ReferenceNumber;";

}
