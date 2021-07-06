namespace Zaabee.Dapper.Extensions.Adapters
{
    internal class SqLiteAdapter : DefaultSqlAdapter
    {
        protected override string FormatColumnName(string columnName) => $"\"{columnName}\"";
    }
}