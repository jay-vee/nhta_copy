set nocount on

-- first obtain all foreign keys and delete
declare @fkTableName varchar(255)
declare @fkConstName varchar(255)

declare cursor1 cursor for
select object_name (fkeyid), object_name (constid) from sysreferences

open cursor1

fetch next from cursor1
into @fkTableName, @fkConstName

while @@fetch_status = 0
begin
  exec ('ALTER TABLE ' + @fkTableName + ' DROP CONSTRAINT ' + @fkConstName)
  fetch next from cursor1
  into @fkTableName, @fkConstName
end

close cursor1
deallocate cursor1


-- now do the same for tables
declare @pkTableName varchar(255)

declare cursor2 cursor for
select object_name (id) from sysobjects where xtype = 'U'

open cursor2

fetch next from cursor2
into @pkTableName
while @@fetch_status = 0
begin
  exec ('DROP TABLE ' + @pkTableName)
  fetch next from cursor2
  into @pkTableName
end

close cursor2
deallocate cursor2