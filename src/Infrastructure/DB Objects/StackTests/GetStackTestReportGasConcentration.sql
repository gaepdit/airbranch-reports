USE airbranch;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE air.GetStackTestReportGasConcentration
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves detailed information for a "Gas Concentration" type 
            stack test.

Input Parameters:
    @ReferenceNumber - The stack test reference number

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2022-02-22  DWaldron            Initial version

*******************************************************************************/

BEGIN
    SET NOCOUNT ON;

    declare @InvalidKey varchar(5) = '00000';

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
            and u1.STRUNITKEY <> @InvalidKey
        left join LOOKUPUNITS u2
        on u2.STRUNITKEY = d.STROPERATINGCAPACITYUNIT
            and u2.STRUNITKEY <> @InvalidKey
        left join LOOKUPUNITS u3
        on u3.STRUNITKEY = d.STRPOLLUTANTCONCENTRATIONUNIT
            and u3.STRUNITKEY <> @InvalidKey
        left join LOOKUPUNITS u4
        on u4.STRUNITKEY = d.STRTREATMENTRATEUNIT
            and u4.STRUNITKEY <> @InvalidKey
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
            and u.STRUNITKEY <> @InvalidKey
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

    declare @params nvarchar(max) = concat_ws(':', '@ReferenceNumber', @ReferenceNumber);
    exec air.LogReport 'StackTestReportGasConcentration', @params;

    return 0;
END;

GO