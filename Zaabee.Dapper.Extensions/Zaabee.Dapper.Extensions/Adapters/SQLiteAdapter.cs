namespace Zaabee.Dapper.Extensions.Adapters
{
    public class SQLiteAdapter : DefaultSqlAdapter
    {
        public override string FormatColumnName(string columnName)
        {
            return $"\"{columnName}\"";
        }
    }
}