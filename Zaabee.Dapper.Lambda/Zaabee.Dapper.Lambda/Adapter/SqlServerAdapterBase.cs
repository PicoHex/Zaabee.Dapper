/* License: http://www.apache.org/licenses/LICENSE-2.0 */

namespace Zaabee.Dapper.Lambda.Adapter
{
    /// <summary>
    /// Provides functionality common to all supported SQL Server versions
    /// </summary>
    class SqlServerAdapterBase : SqlAdapterBase
    {
        public string QueryStringPage(string source, string selection, string conditions, string order,
            int pageSize) => $"SELECT TOP({pageSize}) {selection} FROM {source} {conditions} {order}";

        public string Table(string tableName) => $"[{tableName}]";

        public string Field(string tableName, string fieldName) => $"[{tableName}].[{fieldName}]";

        public string Parameter(string parameterId) => "@" + parameterId;
    }
}