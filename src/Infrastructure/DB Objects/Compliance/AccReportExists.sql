USE airbranch;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE air.AccReportExists
    @AirsNumber varchar(12),
    @Year       int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Reports whether an ACC exists for a given facility and year.

Input Parameters:
    @AirsNumber - The facility ID
    @Year - The ACC year

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2022-02-22  DWaldron            Initial version

*******************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select convert(bit, count(*))
    from dbo.SSCPITEMMASTER m
        inner join dbo.SSCPACCS c
        on m.STRTRACKINGNUMBER = c.STRTRACKINGNUMBER
    where STRAIRSNUMBER = @AirsNumber
      and year(DATACCREPORTINGYEAR) = @Year;

    return 0;
END;

GO