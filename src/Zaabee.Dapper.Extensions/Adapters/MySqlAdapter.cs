namespace Zaabee.Dapper.Extensions.Adapters
{
    internal class MySqlAdapter : DefaultSqlAdapter
    {
        protected override string FormatColumnName(string columnName)
        {
            return $"'{columnName}'";
        }
    }
}