USE airbranch;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE air.StackTestReportExists
    @AirsNumber      varchar(12),
    @ReferenceNumber int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Reports whether a stack test exists 
            for a given facility and reference number.

Input Parameters:
    @AirsNumber - The Facility ID
    @ReferenceNumber - The stack test reference number

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2022-02-22  DWaldron            Initial version

*******************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select convert(bit, count(1))
    from dbo.ISMPMASTER m
        inner join ISMPREPORTINFORMATION r
        on m.STRREFERENCENUMBER = r.STRREFERENCENUMBER
    where r.STRDOCUMENTTYPE <> '001'
      and m.STRAIRSNUMBER = @AirsNumber
      and convert(int, m.STRREFERENCENUMBER) = @ReferenceNumber

    return 0;
END;

GO