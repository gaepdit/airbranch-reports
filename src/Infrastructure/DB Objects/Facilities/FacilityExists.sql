USE airbranch;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE air.FacilityExists
    @AirsNumber varchar(12)
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Reports whether facility with a given ID exists.

Input Parameters:
    @AirsNumber - The facility ID

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2022-02-22  DWaldron            Initial version

*******************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select convert(bit, count(*))
    from dbo.AFSFACILITYDATA
    where STRAIRSNUMBER = @AirsNumber
      and STRUPDATESTATUS <> 'H'

    return 0;
END;

GO