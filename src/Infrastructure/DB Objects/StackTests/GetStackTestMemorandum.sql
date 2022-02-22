USE airbranch;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE air.GetStackTestMemorandum
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves detailed information for a "Memorandum" type stack test.

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
           trim(char(13) + char(10) + ' ' from d.STRMEMORANDUMFIELD)
                                                 as Comments,
           trim(d.STRMONITORMANUFACTUREANDMODEL) as MonitorManufacturer,
           trim(d.STRMONITORSERIALNUMBER)        as MonitorSerialNumber,
           'MaxOperatingCapacity'                as Id,
           d.STRMAXOPERATINGCAPACITY             as Value,
           u1.STRUNITDESCRIPTION                 as Units,
           'OperatingCapacity'                   as Id,
           d.STROPERATINGCAPACITY                as Value,
           u1.STRUNITDESCRIPTION                 as Units
    from ISMPREPORTINFORMATION r
        inner join ISMPREPORTMEMO d
        on d.STRREFERENCENUMBER = r.STRREFERENCENUMBER
        left join LOOKUPUNITS u1
        on u1.STRUNITKEY = d.STRMAXOPERATINGCAPACITYUNIT
            and u1.STRUNITKEY <> '00000'
        left join LOOKUPUNITS u2
        on u2.STRUNITKEY = d.STROPERATINGCAPACITYUNIT
            and u2.STRUNITKEY <> '00000'
    where convert(int, r.STRREFERENCENUMBER) = @ReferenceNumber;

    select trim(t.Value)        as Value,
           u.STRUNITDESCRIPTION as Units
    from (
        select STRREFERENCENUMBER,
               1                              as Id,
               STRALLOWABLEEMISSIONRATE1A     as Value,
               STRALLOWABLEEMISSIONRATEUNIT1A as UnitCode
        from ISMPREPORTMEMO
        union
        select STRREFERENCENUMBER,
               2 as Id,
               STRALLOWABLEEMISSIONRATE1B,
               STRALLOWABLEEMISSIONRATEUNIT1B
        from ISMPREPORTMEMO
        union
        select STRREFERENCENUMBER,
               3 as Id,
               STRALLOWABLEEMISSIONRATE1C,
               STRALLOWABLEEMISSIONRATEUNIT1C
        from ISMPREPORTMEMO
    ) t
        inner join LOOKUPUNITS u
        on u.STRUNITKEY = t.UnitCode
            and u.STRUNITKEY <> '00000'
    where convert(int, t.STRREFERENCENUMBER) = @ReferenceNumber
    order by t.Id;

    declare @params nvarchar(max) = concat_ws(':', '@ReferenceNumber', @ReferenceNumber);
    exec air.LogReport 'StackTestReportMemorandum', @params;

    return 0;
END;

GO