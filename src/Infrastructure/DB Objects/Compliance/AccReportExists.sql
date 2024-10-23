USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.AccReportExists
    @AirsNumber varchar(12),
    @Id       int
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Reports whether an ACC exists for a given facility and year.

Input Parameters:
    @AirsNumber - The facility ID
    @Id - The tracking number of the ACC

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2022-02-22  DWaldron            Initial version
2024-10-22  DWaldron            Exclude deleted ACCs (#106)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select convert(bit, count(*))
    from dbo.SSCPITEMMASTER m
        inner join dbo.SSCPACCS c
        on m.STRTRACKINGNUMBER = c.STRTRACKINGNUMBER
    where m.STRAIRSNUMBER = @AirsNumber
      and c.STRTRACKINGNUMBER = @Id
      and m.STRDELETE is null;

END
GO
