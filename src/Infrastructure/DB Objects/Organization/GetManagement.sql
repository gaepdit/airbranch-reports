USE airbranch;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE air.GetManagement
    @Type varchar(20)
AS

/*******************************************************************************

Author:     Doug Waldron
Overview:   Retrieves the name of the current manager of the given type.

Input Parameters:
    @Type - The type of management (see allowable types)

Allowable types:
    DnrCommissioner
    EpdDirector
    ApbBranchChief
    IsmpProgramManager
    SscpProgramManager
    SsppProgramManager

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2022-02-22  DWaldron            Initial version

*******************************************************************************/

BEGIN
    SET NOCOUNT ON;

    select STRMANAGEMENTNAME
    from LOOKUPAPBMANAGEMENTTYPE
    where STRKEY = @Type
      and STRCURRENTCONTACT = 'C';

    return 0;
END;

GO