﻿USE airbranch;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE air.GetStackTestReportFlare
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves detailed information for a "Flare" type stack test.

Input Parameters:
    @ReferenceNumber - The stack test reference number

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2022-02-22  DWaldron            Initial version

*******************************************************************************/

BEGIN
    SET NOCOUNT ON;

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

    return 0;
END;

GO