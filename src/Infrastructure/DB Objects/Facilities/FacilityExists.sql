USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER PROCEDURE air.FacilityExists
    @AirsNumber varchar(12)
AS

/**************************************************************************************************

Author:     Doug Waldron
Overview:   Reports whether facility with a given ID exists.

Input Parameters:
    @AirsNumber - The facility ID

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2022-02-22  DWaldron            Initial version
2022-12-08  DWaldron            Update filter for approved facilities (IAIP-1177)

***************************************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select convert(bit, count(*))
    from dbo.AFSFACILITYDATA
    where STRAIRSNUMBER = @AirsNumber
      and STRUPDATESTATUS in ('A', 'C');

END;

GO