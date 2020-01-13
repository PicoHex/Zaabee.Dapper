/* License: http://www.apache.org/licenses/LICENSE-2.0 */

namespace Zaabee.Dapper.Lambda.Adapter
{
    /// <summary>
    /// Provides functionality specific to SQL Server 2012
    /// </summary>
    class SqlServer2012Adapter : SqlServerAdapterBase, ISqlAdapter
    {
        public string QueryStringPage(string source, string selection, string conditions, string order,
            int pageSize, int pageNumber) =>
            $"SELECT {selection} FROM {source} {conditions} {order} OFFSET {pageSize * (pageNumber - 1)} ROWS FETCH NEXT {pageSize} ROWS ONLY";
    }
}