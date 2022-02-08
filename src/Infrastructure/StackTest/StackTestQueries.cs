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
       trim(d.STRDESTRUCTIONPERCENT)       as DestructionEfficiency,
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

    public const string StackTestReportPondTreatment = @"
select trim(char(13) + char(10) + ' ' from r.STRCONTROLEQUIPMENTDATA)
                                       as ControlEquipmentInfo,
       d.STRPERCENTALLOWABLE           as DestructionEfficiency,
       'MaxOperatingCapacity'          as Id,
       trim(d.STRMAXOPERATINGCAPACITY) as Value,
       u1.STRUNITDESCRIPTION           as Units,
       'OperatingCapacity'             as Id,
       trim(d.STROPERATINGCAPACITY)    as Value,
       u2.STRUNITDESCRIPTION           as Units,
       'AvgPollutantCollectionRate'    as Id,
       d.STRPOLLUTANTCONCENTRATIONAVG  as Value,
       u3.STRUNITDESCRIPTION           as Units,
       'AvgTreatmentRate'              as Id,
       d.STRTREATMENTRATEAVG           as Value,
       u4.STRUNITDESCRIPTION           as Units
from ISMPREPORTINFORMATION r
    inner join ISMPREPORTPONDANDGAS d
    on d.STRREFERENCENUMBER = r.STRREFERENCENUMBER
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
    on u4.STRUNITKEY = d.STRTREATMENTRATEUNIT
        and u4.STRUNITKEY <> '00000'
where convert(int, r.STRREFERENCENUMBER) = @ReferenceNumber;

select trim(RunNumber)                        as RunNumber,
       trim(PollutantCollectionRate)          as PollutantCollectionRate,
       trim(TreatmentRate)                    as TreatmentRate,
       isnull(ConfidentialParametersCode, '') as ConfidentialParametersCode
from (
    select s.STRREFERENCENUMBER,
           STRRUNNUMBER1A                          as RunNumber,
           s.STRPOLLUTANTCONCENTRATION1A           as PollutantCollectionRate,
           s.STRTREATMENTRATE1A                    as TreatmentRate,
           substring(r.STRCONFIDENTIALDATA, 33, 3) as ConfidentialParametersCode
    from ISMPREPORTPONDANDGAS s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           STRRUNNUMBER1B                          as RunNumber,
           s.STRPOLLUTANTCONCENTRATION1B           as HeatingValue,
           s.STRTREATMENTRATE1B                    as EmissionRateVelocity,
           substring(r.STRCONFIDENTIALDATA, 36, 3) as ConfidentialParametersCode
    from ISMPREPORTPONDANDGAS s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           STRRUNNUMBER1C                          as RunNumber,
           s.STRPOLLUTANTCONCENTRATION1C           as HeatingValue,
           s.STRTREATMENTRATE1C                    as EmissionRateVelocity,
           substring(r.STRCONFIDENTIALDATA, 39, 3) as ConfidentialParametersCode
    from ISMPREPORTPONDANDGAS s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
) t
where convert(int, STRREFERENCENUMBER) = @ReferenceNumber
order by t.RunNumber;
";

    public const string StackTestReportGasConcentration = @"
select trim(char(13) + char(10) + ' ' from r.STRCONTROLEQUIPMENTDATA)
                                       as ControlEquipmentInfo,
       d.STRPERCENTALLOWABLE           as PercentAllowable,
       'MaxOperatingCapacity'          as Id,
       trim(d.STRMAXOPERATINGCAPACITY) as Value,
       u1.STRUNITDESCRIPTION           as Units,
       'OperatingCapacity'             as Id,
       trim(d.STROPERATINGCAPACITY)    as Value,
       u2.STRUNITDESCRIPTION           as Units,
       'AvgPollutantConcentration'     as Id,
       d.STRPOLLUTANTCONCENTRATIONAVG  as Value,
       u3.STRUNITDESCRIPTION           as Units,
       'AvgEmissionRate'               as Id,
       d.STREMISSIONRATEAVG            as Value,
       u4.STRUNITDESCRIPTION           as Units
from ISMPREPORTINFORMATION r
    inner join ISMPREPORTPONDANDGAS d
    on d.STRREFERENCENUMBER = r.STRREFERENCENUMBER
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
    on u4.STRUNITKEY = d.STRTREATMENTRATEUNIT
        and u4.STRUNITKEY <> '00000'
where convert(int, r.STRREFERENCENUMBER) = @ReferenceNumber;

select trim(t.Value)        as Value,
       u.STRUNITDESCRIPTION as Units
from (
    select STRREFERENCENUMBER,
           1                             as Id,
           STRALLOWABLEEMISSIONRATE1     as Value,
           STRALLOWABLEEMISSIONRATEUNIT1 as UnitCode
    from ISMPREPORTPONDANDGAS
    union
    select STRREFERENCENUMBER,
           2 as Id,
           STRALLOWABLEEMISSIONRATE2,
           STRALLOWABLEEMISSIONRATEUNIT2
    from ISMPREPORTPONDANDGAS
    union
    select STRREFERENCENUMBER,
           3 as Id,
           STRALLOWABLEEMISSIONRATE3,
           STRALLOWABLEEMISSIONRATEUNIT3
    from ISMPREPORTPONDANDGAS) t
    inner join LOOKUPUNITS u
    on u.STRUNITKEY = t.UnitCode
        and u.STRUNITKEY <> '00000'
where convert(int, t.STRREFERENCENUMBER) = @ReferenceNumber
order by t.Id;

select trim(RunNumber)                        as RunNumber,
       trim(PollutantConcentration)           as PollutantConcentration,
       trim(EmissionRate)                     as EmissionRate,
       isnull(ConfidentialParametersCode, '') as ConfidentialParametersCode
from (
    select s.STRREFERENCENUMBER,
           STRRUNNUMBER1A                          as RunNumber,
           s.STRPOLLUTANTCONCENTRATION1A           as PollutantConcentration,
           s.STREMISSIONRATE1A                     as EmissionRate,
           substring(r.STRCONFIDENTIALDATA, 33, 3) as ConfidentialParametersCode
    from ISMPREPORTPONDANDGAS s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           STRRUNNUMBER1B,
           s.STRPOLLUTANTCONCENTRATION1B,
           s.STREMISSIONRATE1B,
           substring(r.STRCONFIDENTIALDATA, 36, 3)
    from ISMPREPORTPONDANDGAS s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           STRRUNNUMBER1C,
           s.STRPOLLUTANTCONCENTRATION1C,
           s.STREMISSIONRATE1C,
           substring(r.STRCONFIDENTIALDATA, 39, 3)
    from ISMPREPORTPONDANDGAS s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
) t
where convert(int, STRREFERENCENUMBER) = @ReferenceNumber
order by t.RunNumber;
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
           '2'                                     as RunNumber,
           s.STRHEATINGVALUE2A                     as HeatingValue,
           s.STRVELOCITY2A                         as EmissionRateVelocity,
           substring(r.STRCONFIDENTIALDATA, 35, 3) as ConfidentialParametersCode
    from ISMPREPORTFLARE s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           '3'                                     as RunNumber,
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

    public const string StackTestReportRata = @"
select trim(char(13) + char(10) + ' ' from r.STRCONTROLEQUIPMENTDATA)
                                      as ControlEquipmentInfo,
       d.STRAPPLICABLESTANDARD        as ApplicableStandard,
       p.STRPOLLUTANTDESCRIPTION      as Diluent,
       u.STRUNITDESCRIPTION           as Units,
       d.STRACCURACYCHOICE            as RelativeAccuracyCode,
       d.STRRELATIVEACCURACYPERCENT   as RelativeAccuracyPercent,
       d.STRACCURACYREQUIREDPERCENT   as RelativeAccuracyRequiredPercent,
       d.STRACCURACYREQUIREDSTATEMENT as RelativeAccuracyRequiredLabel,
       case
           when r.STRCOMPLIANCESTATUS in ('02', '03') then 'Pass'
           when r.STRCOMPLIANCESTATUS in ('05') then 'Fail'
           else 'N/A'
       end                            as ComplianceStatus
from ISMPREPORTINFORMATION r
    inner join ISMPREPORTRATA d
    on d.STRREFERENCENUMBER = r.STRREFERENCENUMBER
    left join LOOKUPUNITS u
    on u.STRUNITKEY = d.STRRATAUNITS
    left join LOOKUPPOLLUTANTS p
    on p.STRPOLLUTANTCODE = d.STRDILUENT
where convert(int, r.STRREFERENCENUMBER) = @ReferenceNumber;

select trim(RunNumber)                 as RunNumber,
       trim(ReferenceMethod)           as ReferenceMethod,
       trim(Cms)                       as Cms,
       Omitted,
       IIF(ConfidentialParametersCode = '0', '',
           ConfidentialParametersCode) as ConfidentialParametersCode
from (
    select s.STRREFERENCENUMBER,
           '1'                   as RunNumber,
           s.STRREFERENCEMETHOD1 as ReferenceMethod,
           s.STRCMS1             as Cms,
           convert(bit, substring(STRRUNSINCLUDEDKEY, 1, 1))
                                 as Omitted,
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 29, 1),
                  substring(r.STRCONFIDENTIALDATA, 41, 1))
                                 as ConfidentialParametersCode
    from ISMPREPORTRATA s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           '2',
           s.STRREFERENCEMETHOD2,
           s.STRCMS2,
           convert(bit, substring(STRRUNSINCLUDEDKEY, 2, 1)),
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 30, 1),
                  substring(r.STRCONFIDENTIALDATA, 42, 1))
    from ISMPREPORTRATA s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           '3',
           s.STRREFERENCEMETHOD3,
           s.STRCMS3,
           convert(bit, substring(STRRUNSINCLUDEDKEY, 3, 1)),
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 31, 1),
                  substring(r.STRCONFIDENTIALDATA, 43, 1))
    from ISMPREPORTRATA s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           '4',
           s.STRREFERENCEMETHOD4,
           s.STRCMS4,
           convert(bit, substring(STRRUNSINCLUDEDKEY, 4, 1)),
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 32, 1),
                  substring(r.STRCONFIDENTIALDATA, 44, 1))
    from ISMPREPORTRATA s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           '5',
           s.STRREFERENCEMETHOD5,
           s.STRCMS5,
           convert(bit, substring(STRRUNSINCLUDEDKEY, 5, 1)),
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 33, 1),
                  substring(r.STRCONFIDENTIALDATA, 45, 1))
    from ISMPREPORTRATA s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           '6',
           s.STRREFERENCEMETHOD6,
           s.STRCMS6,
           convert(bit, substring(STRRUNSINCLUDEDKEY, 6, 1)),
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 34, 1),
                  substring(r.STRCONFIDENTIALDATA, 46, 1))
    from ISMPREPORTRATA s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           '7',
           s.STRREFERENCEMETHOD7,
           s.STRCMS7,
           convert(bit, substring(STRRUNSINCLUDEDKEY, 7, 1)),
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 35, 1),
                  substring(r.STRCONFIDENTIALDATA, 47, 1))
    from ISMPREPORTRATA s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           '8',
           s.STRREFERENCEMETHOD8,
           s.STRCMS8,
           convert(bit, substring(STRRUNSINCLUDEDKEY, 8, 1)),
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 36, 1),
                  substring(r.STRCONFIDENTIALDATA, 48, 1))
    from ISMPREPORTRATA s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           '9',
           s.STRREFERENCEMETHOD9,
           s.STRCMS9,
           convert(bit, substring(STRRUNSINCLUDEDKEY, 9, 1)),
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 37, 1),
                  substring(r.STRCONFIDENTIALDATA, 49, 1))
    from ISMPREPORTRATA s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           '10',
           s.STRREFERENCEMETHOD10,
           s.STRCMS10,
           convert(bit, substring(STRRUNSINCLUDEDKEY, 10, 1)),
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 38, 1),
                  substring(r.STRCONFIDENTIALDATA, 50, 1))
    from ISMPREPORTRATA s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           '11',
           s.STRREFERENCEMETHOD11,
           s.STRCMS11,
           convert(bit, substring(STRRUNSINCLUDEDKEY, 11, 1)),
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 39, 1),
                  substring(r.STRCONFIDENTIALDATA, 51, 1))
    from ISMPREPORTRATA s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
    union
    select s.STRREFERENCENUMBER,
           '12',
           s.STRREFERENCEMETHOD12,
           s.STRCMS12,
           convert(bit, substring(STRRUNSINCLUDEDKEY, 12, 1)),
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 40, 1),
                  substring(r.STRCONFIDENTIALDATA, 52, 1))
    from ISMPREPORTRATA s
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = s.STRREFERENCENUMBER
) t
where convert(int, STRREFERENCENUMBER) = @ReferenceNumber
  and not (trim(t.ReferenceMethod) in ('', 'N/A')
    and trim(t.Cms) in ('', 'N/A'))
order by convert(int, t.RunNumber);
";

    public const string StackTestReportOpacity = @"
select trim(char(13) + char(10) + ' ' from r.STRCONTROLEQUIPMENTDATA)
                                  as ControlEquipmentInfo,
       s.STRCOMPLIANCESTATUS      as ComplianceStatus,
       iif(d.STROPACITYSTANDARD = '6', 'Highest 6-minute average',
           '30-minute average')   as OpacityStandard,
       iif(trim(d.STROPACITYTESTDURATION) = '', '',
           concat(trim(d.STROPACITYTESTDURATION), ' minutes'))
                                  as TestDuration,
       u1.STRUNITDESCRIPTION      as MaxOperatingCapacityUnits,
       u2.STRUNITDESCRIPTION      as OperatingCapacityUnits,
       iif(r.STRDOCUMENTTYPE = '015', '',
           u3.STRUNITDESCRIPTION) as AllowableEmissionRateUnits
from ISMPREPORTINFORMATION r
    inner join ISMPREPORTOPACITY d
    on d.STRREFERENCENUMBER = r.STRREFERENCENUMBER
    left join LOOKUPISMPCOMPLIANCESTATUS s
    on s.STRCOMPLIANCEKEY = r.STRCOMPLIANCESTATUS
    left join LOOKUPUNITS u1
    on u1.STRUNITKEY = d.STRMAXOPERATINGCAPACITYUNIT
        and u1.STRUNITKEY <> '00000'
    left join LOOKUPUNITS u2
    on u2.STRUNITKEY = d.STROPERATINGCAPACITYUNIT
        and u2.STRUNITKEY <> '00000'
    left join LOOKUPUNITS u3
    on u3.STRUNITKEY = d.STRALLOWABLEEMISSIONRATEUNIT
        and u3.STRUNITKEY <> '00000'
where convert(int, r.STRREFERENCENUMBER) = @ReferenceNumber;

select RunNumber,
       trim(MaxOperatingCapacity)      as MaxOperatingCapacity,
       trim(OperatingCapacity)         as OperatingCapacity,
       trim(AllowableEmissionRate)     as AllowableEmissionRate,
       trim(Opacity)                   as Opacity,
       trim(AccumulatedEmissionTime)   as AccumulatedEmissionTime,
       trim(EquipmentItem)             as EquipmentItem,
       iif(ConfidentialParametersCode = '00', '',
           ConfidentialParametersCode) as ConfidentialParametersCode
from (
    -- Method 22
    select d.STRREFERENCENUMBER,
           1                            as RunNumber,
           d.STRMAXOPERATINGCAPACITY1A  as MaxOperatingCapacity,
           d.STROPERATINGCAPACITY1A     as OperatingCapacity,
           d.STRALLOWABLEEMISSIONRATE22 as AllowableEmissionRate,
           ''                           as Opacity,
           d.STRACCUMULATEDEMISSIONTIME as AccumulatedEmissionTime,
           ''                           as EquipmentItem,
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 26, 1),
                  substring(r.STRCONFIDENTIALDATA, 27, 1),
                  '0')
                                        as ConfidentialParametersCode
    from ISMPREPORTOPACITY d
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = d.STRREFERENCENUMBER
            and r.STRDOCUMENTTYPE = '015'
    union
    -- Method 9 (Single and Multi)
    select d.STRREFERENCENUMBER,
           1,
           d.STRMAXOPERATINGCAPACITY1A,
           d.STROPERATINGCAPACITY1A,
           d.STRALLOWABLEEMISSIONRATE1A,
           d.STROPACITYPOINTA,
           '',
           d.STREQUIPMENTITEM1,
           iif(r.STRDOCUMENTTYPE = '016',
               concat('0', -- Method 9 Single
                      substring(r.STRCONFIDENTIALDATA, 26, 1),
                      substring(r.STRCONFIDENTIALDATA, 27, 1),
                      '0'),
               concat('0', -- Method 9 Multi
                      substring(r.STRCONFIDENTIALDATA, 26, 1),
                      substring(r.STRCONFIDENTIALDATA, 32, 1),
                      substring(r.STRCONFIDENTIALDATA, 52, 1)))
    from ISMPREPORTOPACITY d
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = d.STRREFERENCENUMBER
            and r.STRDOCUMENTTYPE in ('014', '016')
    union
    -- Method 9 Multi follows
    select d.STRREFERENCENUMBER,
           2,
           d.STRMAXOPERATINGCAPACITY2A,
           d.STROPERATINGCAPACITY2A,
           d.STRALLOWABLEEMISSIONRATE2A,
           d.STROPACITYPOINTB,
           '',
           d.STREQUIPMENTITEM2,
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 27, 1),
                  substring(r.STRCONFIDENTIALDATA, 33, 1),
                  substring(r.STRCONFIDENTIALDATA, 53, 1))
    from ISMPREPORTOPACITY d
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = d.STRREFERENCENUMBER
            and r.STRDOCUMENTTYPE in ('014', '016')
    union
    select d.STRREFERENCENUMBER,
           3,
           d.STRMAXOPERATINGCAPACITY3A,
           d.STROPERATINGCAPACITY3A,
           d.STRALLOWABLEEMISSIONRATE3A,
           d.STROPACITYPOINTC,
           '',
           d.STREQUIPMENTITEM3,
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 28, 1),
                  substring(r.STRCONFIDENTIALDATA, 34, 1),
                  substring(r.STRCONFIDENTIALDATA, 54, 1))
    from ISMPREPORTOPACITY d
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = d.STRREFERENCENUMBER
            and r.STRDOCUMENTTYPE in ('014', '016')
    union
    select d.STRREFERENCENUMBER,
           4,
           d.STRMAXOPERATINGCAPACITY4A,
           d.STROPERATINGCAPACITY4A,
           d.STRALLOWABLEEMISSIONRATE4A,
           d.STROPACITYPOINTD,
           '',
           d.STREQUIPMENTITEM4,
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 25, 1),
                  substring(r.STRCONFIDENTIALDATA, 35, 1),
                  substring(r.STRCONFIDENTIALDATA, 55, 1))
    from ISMPREPORTOPACITY d
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = d.STRREFERENCENUMBER
            and r.STRDOCUMENTTYPE in ('014', '016')
    union
    select d.STRREFERENCENUMBER,
           5,
           d.STRMAXOPERATINGCAPACITY5A,
           d.STROPERATINGCAPACITY5A,
           d.STRALLOWABLEEMISSIONRATE5A,
           d.STROPACITYPOINTE,
           '',
           d.STREQUIPMENTITEM5,
           concat('0',
                  substring(r.STRCONFIDENTIALDATA, 30, 1),
                  substring(r.STRCONFIDENTIALDATA, 36, 1),
                  substring(r.STRCONFIDENTIALDATA, 56, 1))
    from ISMPREPORTOPACITY d
        inner join ISMPREPORTINFORMATION r
        on r.STRREFERENCENUMBER = d.STRREFERENCENUMBER
            and r.STRDOCUMENTTYPE in ('014', '016')
) t
where convert(int, STRREFERENCENUMBER) = @ReferenceNumber
  and not (t.RunNumber > 1 and trim(t.Opacity) in ('', 'N/A'))
order by t.RunNumber;
";

}
