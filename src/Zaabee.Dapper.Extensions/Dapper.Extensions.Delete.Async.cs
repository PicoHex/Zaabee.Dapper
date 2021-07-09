using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Zaabee.Dapper.Extensions.Enums;

namespace Zaabee.Dapper.Extensions
{
    public static partial class DapperExtensions
    {
        public static Task<int> DeleteAsync<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var id = TypeMapInfoHelper.GetIdValue(persistentObject);
            return DeleteAsync<T>(connection, id, transaction, commandTimeout, commandType);
        }

        public static Task<int> DeleteAsync<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.ExecuteAsync(
                adapter.GetDeleteSql(typeof(T), CriteriaType.SingleId),
                new {Id = id}, transaction, commandTimeout, commandType);
        }

        public static Task<int> DeleteAllAsync<T>(this IDbConnection connection, IList<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.ExecuteAsync(adapter.GetDeleteSql(typeof(T), CriteriaType.SingleId),
                persistentObjects, transaction, commandTimeout, commandType);
        }

        public static Task<int> DeleteAllAsync<T>(this IDbConnection connection, object ids,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.ExecuteAsync(adapter.GetDeleteSql(typeof(T), CriteriaType.MultiId),
                new {Ids = (IEnumerable) ids}, transaction, commandTimeout, commandType);
        }

        public static Task<int> DeleteAllAsync<T>(this IDbConnection connection,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.ExecuteAsync(adapter.GetDeleteSql(typeof(T), CriteriaType.None), null, transaction,
                commandTimeout, commandType);
        }
    }
}