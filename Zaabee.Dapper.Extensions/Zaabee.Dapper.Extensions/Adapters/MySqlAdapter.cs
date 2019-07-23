namespace Zaabee.Dapper.Extensions.Adapters
{
    public class MySqlAdapter : DefaultSqlAdapter
    {
        public override string FormatColumnName(string columnName)
        {
            return $"'{columnName}'";
        }
    }
}