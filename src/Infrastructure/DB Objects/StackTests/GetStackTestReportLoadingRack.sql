USE airbranch;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE air.GetStackTestReportLoadingRack
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves detailed information for a "Loading Rack" type stack test.

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

    declare @params nvarchar(max) = concat_ws(':', '@ReferenceNumber', @ReferenceNumber);
    exec air.LogReport 'StackTestReportLoadingRack', @params;

    return 0;
END;

GO