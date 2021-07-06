using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Zaabee.Dapper.Extensions
{
    public static partial class DapperExtensions
    {
        public static async Task<int> UpdateAsync<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.ExecuteAsync(adapter.GetUpdateSql(typeof(T)), persistentObject, transaction,
                commandTimeout, commandType);
        }

        public static async Task<int> UpdateAllAsync<T>(this IDbConnection connection, IList<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.ExecuteAsync(adapter.GetUpdateSql(typeof(T)),
                persistentObjects, transaction, commandTimeout, commandType);
        }
    }
}