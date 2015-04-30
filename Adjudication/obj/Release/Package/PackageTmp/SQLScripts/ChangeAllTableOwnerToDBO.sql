/*
--- FOR SQL 2000 AND EARLIER ---
DECLARE @old sysname, @new sysname, @sql varchar(1000)

SELECT
  @old = 'E210867'
  , @new = 'dbo'
  , @sql = '
  IF EXISTS (SELECT NULL FROM INFORMATION_SCHEMA.TABLES
  WHERE
      QUOTENAME(TABLE_SCHEMA)+''.''+QUOTENAME(TABLE_NAME) = ''?''
      AND TABLE_SCHEMA = ''' + @old + '''
  )
  EXECUTE sp_changeobjectowner ''?'', ''' + @new + ''''

EXECUTE sp_MSforeachtable @sql
*/


--- FOR SQL 2005 ---
DECLARE @old sysname, @sql varchar(1000)

SELECT
 @old = 'E210867'
 , @sql = '
 IF EXISTS (SELECT NULL FROM INFORMATION_SCHEMA.TABLES
 WHERE
     QUOTENAME(TABLE_SCHEMA)+''.''+QUOTENAME(TABLE_NAME) = ''?''
     AND TABLE_SCHEMA = ''' + @old + '''
 )
 ALTER SCHEMA dbo TRANSFER ?'

EXECUTE sp_MSforeachtable @sql
GO