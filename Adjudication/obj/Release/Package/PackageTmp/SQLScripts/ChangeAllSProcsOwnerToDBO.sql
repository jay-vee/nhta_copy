DECLARE @oldOwner sysname, @newOwner sysname

SELECT
    @oldOwner = 'E210867'
    , @newOwner = 'dbo'

select 'EXECUTE sp_changeobjectowner '''+QUOTENAME(a.SPECIFIC_SCHEMA)+'.'+QUOTENAME(a.ROUTINE_NAME)+''','''+@newOwner+''''
from
    INFORMATION_SCHEMA.ROUTINES a
where
    a.ROUTINE_TYPE = 'PROCEDURE'
    AND a.SPECIFIC_SCHEMA = @oldOwner
    AND
OBJECTPROPERTY(OBJECT_ID(QUOTENAME(a.SPECIFIC_SCHEMA)+'.'+QUOTENAME(a.ROUTINE_NAME)), 'IsMSShipped') = 0

GO

