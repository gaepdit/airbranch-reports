namespace Infrastructure.Facilities;

internal static class FacilitiesQueries
{
    public static string FacilityExists = @"
select convert(bit, count(*))
from dbo.AFSFACILITYDATA
where STRAIRSNUMBER = @AirsNumber
  and STRUPDATESTATUS <> 'H'
";

    public static string GetFacility = @"
select f.STRAIRSNUMBER               as Id,
       trim(f.STRFACILITYNAME)       as Name,
       trim(char(13) + char(10) + ' ' from h.STRPLANTDESCRIPTION)
                                     as Description,
       lc.STRCOUNTYNAME              as County,
       'FacilityAddress'             as Id,
       dbo.NullIfNaOrEmpty(f.STRFACILITYSTREET1)
                                     as Street,
       f.STRFACILITYSTREET2          as Street2,
       trim(f.STRFACILITYCITY)       as City,
       f.STRFACILITYSTATE            as State,
       f.STRFACILITYZIPCODE          as PostalCode,
       'GeoCoordinates'              as Id,
       f.NUMFACILITYLATITUDE         as Latitude,
       f.NUMFACILITYLONGITUDE        as Longitude,
       'HeaderData'                  as Id,
       h.STROPERATIONALSTATUS        as OperatingStatusCode,
       h.DATSTARTUPDATE              as StartupDate,
       h.DATSHUTDOWNDATE             as PermitRevocationDate,
       h.STRCLASS                    as ClassificationCode,
       COALESCE(s.STRCMSMEMBER, 'X') as CmsClassificationCode,
       IIF(s.FacilityOwnershipTypeCode = 'FDF',
           'Federal Facility (U.S. Government)', '')
                                     as OwnershipType,
       h.STRSICCODE                  as Sic,
       h.STRNAICSCODE                as Naics,
       s.STRRMPID                    as RmpId,
       convert(int, substring(coalesce(h.STRATTAINMENTSTATUS, '00000'), 2, 1))
                                     as OneHourOzoneNonattainment,
       convert(int, substring(coalesce(h.STRATTAINMENTSTATUS, '00000'), 3, 1))
                                     as EightHourOzoneNonattainment,
       convert(int, substring(coalesce(h.STRATTAINMENTSTATUS, '00000'), 4, 1))
                                     as PmFineNonattainment,
       s.NspsFeeExempt
from dbo.APBFACILITYINFORMATION f
    inner join dbo.APBHEADERDATA h
    on f.STRAIRSNUMBER = h.STRAIRSNUMBER
    inner join APBSUPPLAMENTALDATA s
    on f.STRAIRSNUMBER = s.STRAIRSNUMBER
    inner join dbo.AFSFACILITYDATA a
    on f.STRAIRSNUMBER = a.STRAIRSNUMBER
    left join dbo.LOOKUPCOUNTYINFORMATION lc
    on substring(f.STRAIRSNUMBER, 5, 3) = lc.STRCOUNTYCODE
where f.STRAIRSNUMBER = @AirsNumber
  and a.STRUPDATESTATUS <> 'H';

select t.AirProgram
from (
    select 1             as Sequence,
           STRAIRSNUMBER as FacilityId,
           IIF(substring(STRAIRPROGRAMCODES, 1, 1) = '1', 'SIP', null)
                         as AirProgram
    from dbo.APBHEADERDATA
    union
    select 2,
           STRAIRSNUMBER,
           IIF(substring(STRAIRPROGRAMCODES, 2, 1) = '1', 'Federal SIP', null)
    from dbo.APBHEADERDATA
    union
    select 3,
           STRAIRSNUMBER,
           IIF(substring(STRAIRPROGRAMCODES, 3, 1) = '1', 'Non-Federal SIP', null)
    from dbo.APBHEADERDATA
    union
    select 4,
           STRAIRSNUMBER,
           IIF(substring(STRAIRPROGRAMCODES, 4, 1) = '1', 'CFC Tracking', null)
    from dbo.APBHEADERDATA
    union
    select 5,
           STRAIRSNUMBER,
           IIF(substring(STRAIRPROGRAMCODES, 5, 1) = '1', 'PSD', null)
    from dbo.APBHEADERDATA
    union
    select 6,
           STRAIRSNUMBER,
           IIF(substring(STRAIRPROGRAMCODES, 6, 1) = '1', 'NSR', null)
    from dbo.APBHEADERDATA
    union
    select 9,
           STRAIRSNUMBER,
           IIF(substring(STRAIRPROGRAMCODES, 7, 1) = '1', 'NESHAP', null)
    from dbo.APBHEADERDATA
    union
    select 10,
           STRAIRSNUMBER,
           IIF(substring(STRAIRPROGRAMCODES, 8, 1) = '1', 'NSPS', null)
    from dbo.APBHEADERDATA
    union
    select 12,
           STRAIRSNUMBER,
           IIF(substring(STRAIRPROGRAMCODES, 9, 1) = '1', 'FESOP', null)
    from dbo.APBHEADERDATA
    union
    select 11,
           STRAIRSNUMBER,
           IIF(substring(STRAIRPROGRAMCODES, 10, 1) = '1', 'Acid Precipitation', null)
    from dbo.APBHEADERDATA
    union
    select 13,
           STRAIRSNUMBER,
           IIF(substring(STRAIRPROGRAMCODES, 11, 1) = '1', 'Native American', null)
    from dbo.APBHEADERDATA
    union
    select 8,
           STRAIRSNUMBER,
           IIF(substring(STRAIRPROGRAMCODES, 12, 1) = '1', 'MACT', null)
    from dbo.APBHEADERDATA
    union
    select 7,
           STRAIRSNUMBER,
           IIF(substring(STRAIRPROGRAMCODES, 13, 1) = '1', 'Title V', null)
    from dbo.APBHEADERDATA
    union
    select 14,
           STRAIRSNUMBER,
           IIF(substring(STRAIRPROGRAMCODES, 14, 1) = '1', 'Risk Management Plan', null)
    from dbo.APBHEADERDATA
) t
where t.FacilityId = @AirsNumber
  and AirProgram is not null
order by t.Sequence;

select t.AirProgram
from (
    select 1             as Sequence,
           STRAIRSNUMBER as FacilityId,
           IIF(substring(STRSTATEPROGRAMCODES, 1, 1) = '1', 'NSR/PSD Major', null)
                         as AirProgram
    from dbo.APBHEADERDATA
    union
    select 2,
           STRAIRSNUMBER,
           IIF(substring(STRSTATEPROGRAMCODES, 2, 1) = '1', 'HAPs Major', null)
    from dbo.APBHEADERDATA
) t
where t.FacilityId = @AirsNumber
  and AirProgram is not null
order by t.Sequence;
";

}
