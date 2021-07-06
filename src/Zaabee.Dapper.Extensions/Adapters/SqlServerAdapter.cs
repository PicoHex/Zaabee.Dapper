namespace Zaabee.Dapper.Extensions.Adapters
{
    internal class SqlServerAdapter : DefaultSqlAdapter
    {
        protected override string FormatColumnName(string columnName) => $"[{columnName}]";
    }
}