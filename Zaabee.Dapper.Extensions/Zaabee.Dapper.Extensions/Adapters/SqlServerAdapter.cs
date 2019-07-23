namespace Zaabee.Dapper.Extensions.Adapters
{
    public class SqlServerAdapter : DefaultSqlAdapter
    {
        protected override string FormatColumnName(string columnName)
        {
            return $"[{columnName}]";
        }
    }
}