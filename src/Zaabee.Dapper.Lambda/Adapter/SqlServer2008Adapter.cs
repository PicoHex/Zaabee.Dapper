/* License: http://www.apache.org/licenses/LICENSE-2.0 */

namespace Zaabee.Dapper.Lambda.Adapter
{
    /// <summary>
    /// Provides functionality specific to SQL Server 2008
    /// </summary>
    internal class SqlServer2008Adapter : SqlServerAdapterBase, ISqlAdapter
    {
        public string QueryStringPage(string source, string selection, string conditions, string order,
            int pageSize, int pageNumber)
        {
            var innerQuery = $"SELECT {selection},ROW_NUMBER() OVER ({order}) AS RN FROM {source} {conditions}";

            return
                $"SELECT TOP {pageSize} * FROM ({innerQuery}) InnerQuery WHERE RN > {pageSize * (pageNumber - 1)} ORDER BY RN";
        }
    }
}