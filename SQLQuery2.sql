--create class data properties
select 'public ' + case data_type
when 'varchar' then 'string '
when 'nvarchar' then 'string '
when 'datetime' then 'DateTime '
else data_type + ' '
end + column_name + ' { get; set; }' as property
from INFORMATION_SCHEMA.COLUMNS
Where table_name = 'Cart_LineItems'
Order By ordinal_position