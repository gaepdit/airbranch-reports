namespace Infrastructure.StackTest;

internal static class StackTestQueries
{
    public const string StackTestReportExists = @"
select convert(bit, count(1))
from dbo.ISMPMASTER m
    inner join ISMPREPORTINFORMATION r
    on m.STRREFERENCENUMBER = r.STRREFERENCENUMBER
where r.STRDOCUMENTTYPE <> '001'
  and m.STRAIRSNUMBER = @AirsNumber
  and convert(int, m.STRREFERENCENUMBER) = @ReferenceNumber
";

    public const string GetDocumentType = @"
select convert(int, STRDOCUMENTTYPE) as DocumentType
from ISMPREPORTINFORMATION
where STRREFERENCENUMBER = @ReferenceNumber
";

    public const string BaseStackTestReport = @"
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
where r.STRREFERENCENUMBER = @ReferenceNumber;
";

    public const string StackTestReportOneStack = @"
select trim(char(13) + char(10) + ' ' from r.STRCONTROLEQUIPMENTDATA)
                                        as ControlEquipmentInfo,
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

    public const string StackTestReportTwoStack = @"
select trim(char(13) + char(10) + ' ' from r.STRCONTROLEQUIPMENTDATA)
                                           as ControlEquipmentInfo,
       trim(d.STRSTACKONENAME)             as StackOneName,
       trim(d.STRSTACKTWONAME)             as StackTwoName,
       trim(d.STRPERCENTALLOWABLE)         as PercentAllowable,
       'MaxOperatingCapacity'              as Id,
       trim(d.STRMAXOPERATINGCAPACITY)     as Value,
       u1.STRUNITDESCRIPTION               as Units,
       'OperatingCapacity'                 as Id,
       trim(d.STROPERATINGCAPACITY)        as Value,
       u2.STRUNITDESCRIPTION               as Units,
       'StackOneAvgPollutantConcentration' as Id,
       d.STRPOLLUTANTCONCENTRATIONAVG1     as Value,
       u3.STRUNITDESCRIPTION               as Units,
       'StackTwoAvgPollutantConcentration' as Id,
       d.STRPOLLUTANTCONCENTRATIONAVG2     as Value,
       u3.STRUNITDESCRIPTION               as Units,
       'StackOneAvgEmissionRate'           as Id,
       d.STREMISSIONRATEAVG1               as Value,
       u4.STRUNITDESCRIPTION               as Units,
       'StackTwoAvgEmissionRate'           as Id,
       d.STREMISSIONRATEAVG2               as Value,
       u4.STRUNITDESCRIPTION               as Units,
       'SumAvgEmissionRate'                as Id,
       d.STREMISSIONRATETOTALAVG           as Value,
       u4.STRUNITDESCRIPTION               as Units
from ISMPREPORTINFORMATION r
    inner join ISMPREPORTTWOSTACK d
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

select trim(t.Value)        as Value,
       u.STRUNITDESCRIPTION as Units
from (
    select STRREFERENCENUMBER,
           1                             as Id,
           STRALLOWABLEEMISSIONRATE1     as Value,
           STRALLOWABLEEMISSIONRATEUNIT1 as UnitCode
    from ISMPREPORTTWOSTACK
    union
    select STRREFERENCENUMBER,
           2 as Id,
           STRALLOWABLEEMISSIONRATE2,
           STRALLOWABLEEMISSIONRATEUNIT2
    from ISMPREPORTTWOSTACK
    union
    select STRREFERENCENUMBER,
           3 as Id,
           STRALLOWABLEEMISSIONRATE3,
           STRALLOWABLEEMISSIONRATEUNIT3
    from ISMPREPORTTWOSTACK) t
    inner join LOOKUPUNITS u
    on u.STRUNITKEY = t.UnitCode
        and u.STRUNITKEY <> '00000'
where convert(int, t.STRREFERENCENUMBER) = @ReferenceNumber
order by t.Id;

select trim(RunNumber)                        as RunNumber,
       trim(StackOneGasTemperature)           as StackOneGasTemperature,
       trim(StackOneGasMoisture)              as StackOneGasMoisture,
       trim(StackOneGasFlowRateAscfm)         as StackOneGasFlowRateAscfm,
       trim(StackOneGasFlowRateDscfm)         as StackOneGasFlowRateDscfm,
       trim(StackOnePollutantConcentration)   as StackOnePollutantConcentration,
       trim(StackOneEmissionRate)             as StackOneEmissionRate,
       trim(StackTwoGasTemperature)           as StackTwoGasTemperature,
       trim(StackTwoGasMoisture)              as StackTwoGasMoisture,
       trim(StackTwoGasFlowRateAscfm)         as StackTwoGasFlowRateAscfm,
       trim(StackTwoGasFlowRateDscfm)         as StackTwoGasFlowRateDscfm,
       trim(StackTwoPollutantConcentration)   as StackTwoPollutantConcentration,
       trim(StackTwoEmissionRate)             as StackTwoEmissionRate,
       trim(SumEmissionRate)                  as SumEmissionRate,
       isnull(ConfidentialParametersCode, '') as ConfidentialParametersCode
from (
    select 1                             as Id, -- run 1
           s.STRREFERENCENUMBER          as ReferenceNumber,
           s.STRRUNNUMBER1A              as RunNumber,
           s.STRGASTEMPERATURE1A         as StackOneGasTemperature,
           s.STRGASMOISTURE1A            as StackOneGasMoisture,
           s.STRGASFLOWRATEACFM1A        as StackOneGasFlowRateAscfm,
           s.STRGASFLOWRATEDSCFM1A       as StackOneGasFlowRateDscfm,
           s.STRPOLLUTANTCONCENTRATION1A as StackOnePollutantConcentration,
           s.STREMISSIONRATE1A           as StackOneEmissionRate,
           s.STRGASTEMPERATURE2A         as StackTwoGasTemperature,
           s.STRGASMOISTURE2A            as StackTwoGasMoisture,
           s.STRGASFLOWRATEACFM2A        as StackTwoGasFlowRateAscfm,
           s.STRGASFLOWRATEDSCFM2A       as StackTwoGasFlowRateDscfm,
           s.STRPOLLUTANTCONCENTRATION2A as StackTwoPollutantConcentration,
           s.STREMISSIONRATE2A           as StackTwoEmissionRate,
           s.STREMISSIONRATETOTAL1       as SumEmissionRate,
           concat(substring(r.STRCONFIDENTIALDATA, 36, 7),
                  substring(r.STRCONFIDENTIALDATA, 58, 6),
                  substring(r.STRCONFIDENTIALDATA, 84, 1))
                                         as ConfidentialParametersCode
    from ISMPREPORTTWOSTACK s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select 2, -- run 2
           s.STRREFERENCENUMBER,
           s.STRRUNNUMBER1B,
           s.STRGASTEMPERATURE1B,
           s.STRGASMOISTURE1B,
           s.STRGASFLOWRATEACFM1B,
           s.STRGASFLOWRATEDSCFM1B,
           s.STRPOLLUTANTCONCENTRATION1B,
           s.STREMISSIONRATE1B,
           s.STRGASTEMPERATURE2B,
           s.STRGASMOISTURE2B,
           s.STRGASFLOWRATEACFM2B,
           s.STRGASFLOWRATEDSCFM2B,
           s.STRPOLLUTANTCONCENTRATION2B,
           s.STREMISSIONRATE2B,
           s.STREMISSIONRATETOTAL2,
           concat(substring(r.STRCONFIDENTIALDATA, 43, 7),
                  substring(r.STRCONFIDENTIALDATA, 65, 6),
                  substring(r.STRCONFIDENTIALDATA, 85, 1))
    from ISMPREPORTTWOSTACK s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select 3, -- run 3
           s.STRREFERENCENUMBER,
           s.STRRUNNUMBER1C,
           s.STRGASTEMPERATURE1C,
           s.STRGASMOISTURE1C,
           s.STRGASFLOWRATEACFM1C,
           s.STRGASFLOWRATEDSCFM1C,
           s.STRPOLLUTANTCONCENTRATION1C,
           s.STREMISSIONRATE1C,
           s.STRGASTEMPERATURE2C,
           s.STRGASMOISTURE2C,
           s.STRGASFLOWRATEACFM2C,
           s.STRGASFLOWRATEDSCFM2C,
           s.STRPOLLUTANTCONCENTRATION2C,
           s.STREMISSIONRATE2C,
           s.STREMISSIONRATETOTAL3,
           concat(substring(r.STRCONFIDENTIALDATA, 50, 7),
                  substring(r.STRCONFIDENTIALDATA, 72, 6),
                  substring(r.STRCONFIDENTIALDATA, 86, 1))
    from ISMPREPORTTWOSTACK s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
) t
where convert(int, ReferenceNumber) = @ReferenceNumber
order by Id;
";

    public const string StackTestReportFlare = @"
select trim(char(13) + char(10) + ' ' from r.STRCONTROLEQUIPMENTDATA)
                                       as ControlEquipmentInfo,
       d.STRPERCENTALLOWABLE           as PercentAllowable,
       'MaxOperatingCapacity'          as Id,
       trim(d.STRMAXOPERATINGCAPACITY) as Value,
       u1.STRUNITDESCRIPTION           as Units,
       'OperatingCapacity'             as Id,
       trim(d.STROPERATINGCAPACITY)    as Value,
       u2.STRUNITDESCRIPTION           as Units,
       'AvgHeatingValue'               as Id,
       d.STRHEATINGVALUEAVG            as Value,
       u3.STRUNITDESCRIPTION           as Units,
       'AvgEmissionRateVelocity'       as Id,
       d.STRVELOCITYAVG                as Value,
       u4.STRUNITDESCRIPTION           as Units
from ISMPREPORTINFORMATION r
    inner join ISMPREPORTFLARE d
    on d.STRREFERENCENUMBER = r.STRREFERENCENUMBER
    left join LOOKUPUNITS u1
    on u1.STRUNITKEY = d.STRMAXOPERATINGCAPACITYUNIT
        and u1.STRUNITKEY <> '00000'
    left join LOOKUPUNITS u2
    on u2.STRUNITKEY = d.STROPERATINGCAPACITYUNIT
        and u2.STRUNITKEY <> '00000'
    left join LOOKUPUNITS u3
    on u3.STRUNITKEY = d.STRHEATINGVALUEUNITS
        and u3.STRUNITKEY <> '00000'
    left join LOOKUPUNITS u4
    on u4.STRUNITKEY = d.STRVELOCITYUNITS
        and u4.STRUNITKEY <> '00000'
where convert(int, r.STRREFERENCENUMBER) = @ReferenceNumber;

select trim(t.Value) as Value,
       Units,
       Preamble
from (
    select STRREFERENCENUMBER,
           1                     as Id,
           STRLIMITATIONVELOCITY as Value,
           'ft/sec'              as Units,
           'Velocity less than'  as Preamble
    from ISMPREPORTFLARE
    union
    select STRREFERENCENUMBER,
           2 as Id,
           STRLIMITATIONHEATCAPACITY,
           'BTU/scf',
           'Heat Content greater than or equal to'
    from ISMPREPORTFLARE
) t
where convert(int, t.STRREFERENCENUMBER) = @ReferenceNumber
order by t.Id;

select trim(RunNumber)                        as RunNumber,
       trim(HeatingValue)                     as HeatingValue,
       trim(EmissionRateVelocity)             as EmissionRateVelocity,
       isnull(ConfidentialParametersCode, '') as ConfidentialParametersCode
from (
    select s.STRREFERENCENUMBER,
           '1'                                     as RunNumber,
           s.STRHEATINGVALUE1A                     as HeatingValue,
           s.STRVELOCITY1A                         as EmissionRateVelocity,
           substring(r.STRCONFIDENTIALDATA, 32, 3) as ConfidentialParametersCode
    from ISMPREPORTFLARE s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           '1'                                     as RunNumber,
           s.STRHEATINGVALUE2A                     as HeatingValue,
           s.STRVELOCITY2A                         as EmissionRateVelocity,
           substring(r.STRCONFIDENTIALDATA, 35, 3) as ConfidentialParametersCode
    from ISMPREPORTFLARE s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           '1'                                     as RunNumber,
           s.STRHEATINGVALUE3A                     as HeatingValue,
           s.STRVELOCITY3A                         as EmissionRateVelocity,
           substring(r.STRCONFIDENTIALDATA, 38, 3) as ConfidentialParametersCode
    from ISMPREPORTFLARE s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
) t
where convert(int, STRREFERENCENUMBER) = @ReferenceNumber
order by t.RunNumber;
";

    public const string StackTestReportLoadingRack = @"
select trim(char(13) + char(10) + ' ' from r.STRCONTROLEQUIPMENTDATA)
                                        as ControlEquipmentInfo,
       'MaxOperatingCapacity'           as Id,
       trim(d.STRMAXOPERATINGCAPACITY)  as Value,
       u1.STRUNITDESCRIPTION            as Units,
       'OperatingCapacity'              as Id,
       trim(d.STROPERATINGCAPACITY)     as Value,
       u2.STRUNITDESCRIPTION            as Units,
       'TestDuration'                   as Id,
       trim(d.STRTESTDURATION)          as Value,
       u3.STRUNITDESCRIPTION            as Units,
       'PollutantConcentrationIn'       as Id,
       trim(d.STRPOLLUTANTCONCENIN)     as Value,
       u4.STRUNITDESCRIPTION            as Units,
       'PollutantConcentrationOut'      as Id,
       trim(d.STRPOLLUTANTCONCENOUT)    as Value,
       u5.STRUNITDESCRIPTION            as Units,
       'EmissionRate'                   as Id,
       trim(d.STREMISSIONRATE)          as Value,
       u6.STRUNITDESCRIPTION            as Units,
       'DestructionReduction'           as Id,
       trim(d.STRDESTRUCTIONEFFICIENCY) as Value,
       '%'                              as Units
from ISMPREPORTINFORMATION r
    inner join ISMPREPORTFLARE d
    on d.STRREFERENCENUMBER = r.STRREFERENCENUMBER
    left join LOOKUPUNITS u1
    on u1.STRUNITKEY = d.STRMAXOPERATINGCAPACITYUNIT
        and u1.STRUNITKEY <> '00000'
    left join LOOKUPUNITS u2
    on u2.STRUNITKEY = d.STROPERATINGCAPACITYUNIT
        and u2.STRUNITKEY <> '00000'
    left join LOOKUPUNITS u3
    on u3.STRUNITKEY = d.STRTESTDURATIONUNIT
        and u3.STRUNITKEY <> '00000'
    left join LOOKUPUNITS u4
    on u4.STRUNITKEY = d.STRPOLLUTANTCONCENUNITIN
        and u4.STRUNITKEY <> '00000'
    left join LOOKUPUNITS u5
    on u5.STRUNITKEY = d.STRPOLLUTANTCONCENUNITOUT
        and u5.STRUNITKEY <> '00000'
    left join LOOKUPUNITS u6
    on u6.STRUNITKEY = d.STREMISSIONRATEUNIT
        and u6.STRUNITKEY <> '00000'
where convert(int, r.STRREFERENCENUMBER) = @ReferenceNumber;

select trim(t.Value)        as Value,
       u.STRUNITDESCRIPTION as Units
from (
    select STRREFERENCENUMBER,
           1                          as Id,
           STRALLOWABLEEMISSIONRATE1A as Value,
           STRALLOWEMISSIONRATEUNIT1A as UnitCode
    from ISMPREPORTFLARE
    union
    select STRREFERENCENUMBER,
           2 as Id,
           STRALLOWABLEEMISSIONRATE2A,
           STRALLOWEMISSIONRATEUNIT2A
    from ISMPREPORTFLARE
    union
    select STRREFERENCENUMBER,
           3 as Id,
           STRALLOWABLEEMISSIONRATE3A,
           STRALLOWEMISSIONRATEUNIT3A
    from ISMPREPORTFLARE) t
    inner join LOOKUPUNITS u
    on u.STRUNITKEY = t.UnitCode
        and u.STRUNITKEY <> '00000'
where convert(int, t.STRREFERENCENUMBER) = @ReferenceNumber
order by t.Id;
";

}
