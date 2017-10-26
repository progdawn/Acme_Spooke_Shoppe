--create column related values for Insert and Update statements
Select column_name + ',' as ColumnName,
'@'+column_name+',' as ColumnParameterName,
column_name + ' = @'+column_name+',' as ColumnUpdateSet,
'cmd.Parameters.AddWithValue("@' + column_name + '", SqlDbType.' +
Case DATA_TYPE
When 'int' then 'Int'
When 'decimal' then 'Decimal'
When 'varchar' then 'VarChar'
When 'nvarchar' then 'NVarChar'
When 'datetime' then 'Date'
else 'error'
end +').Value = obj.' + column_name + ';' as ParameterAssignmentStatements
from INFORMATION_SCHEMA.COLUMNS
Where table_name = 'Cart_LineItems'
Order By ordinal_position