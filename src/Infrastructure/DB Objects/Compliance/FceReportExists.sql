USE airbranch;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE air.FceReportExists
    @AirsNumber varchar(12),
    @Id       int
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Reports whether an FCE exists for a given facility and FCE ID.

Input Parameters:
    @AirsNumber - The facility ID
    @Id - The tracking number of the FCE

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2022-02-22  DWaldron            Initial version

*******************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select convert(bit, count(*))
    from SSCPFCEMASTER
    where STRAIRSNUMBER = @AirsNumber
      and STRFCENUMBER = @Id
      and (IsDeleted is null or IsDeleted = 0)

    return 0;
END;

GO