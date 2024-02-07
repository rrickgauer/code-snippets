[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class SqlColumnAttribute : Attribute
{
    public string ColumnName { get; }

    public SqlColumnAttribute([CallerMemberName] string columnName = "")
    {
        ColumnName = columnName;
    }
}