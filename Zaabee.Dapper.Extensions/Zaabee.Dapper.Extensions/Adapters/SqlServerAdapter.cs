namespace Zaabee.Dapper.Extensions.Adapters
{
    public class SqlServerAdapter : DefaultSqlAdapter
    {
        public override string FormatColumnName(string columnName)
        {
            return $"[{columnName}]";
        }
    }
}