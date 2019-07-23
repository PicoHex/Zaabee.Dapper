namespace Zaabee.Dapper.Extensions.Adapters
{
    public class SQLiteAdapter : DefaultSqlAdapter
    {
        protected override string FormatColumnName(string columnName)
        {
            return $"\"{columnName}\"";
        }
    }
}