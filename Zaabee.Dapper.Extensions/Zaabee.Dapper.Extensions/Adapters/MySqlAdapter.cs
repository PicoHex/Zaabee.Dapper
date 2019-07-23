namespace Zaabee.Dapper.Extensions.Adapters
{
    public class MySqlAdapter : DefaultSqlAdapter
    {
        protected override string FormatColumnName(string columnName)
        {
            return $"'{columnName}'";
        }
    }
}