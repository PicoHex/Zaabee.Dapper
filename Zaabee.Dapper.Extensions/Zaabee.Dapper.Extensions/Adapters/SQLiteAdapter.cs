namespace Zaabee.Dapper.Extensions.Adapters
{
    internal class SQLiteAdapter : DefaultSqlAdapter
    {
        protected override string FormatColumnName(string columnName)
        {
            return $"\"{columnName}\"";
        }
    }
}