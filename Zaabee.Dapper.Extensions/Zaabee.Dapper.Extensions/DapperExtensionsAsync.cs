using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Zaabee.Dapper.Extensions.Enums;

namespace Zaabee.Dapper.Extensions
{
    public static partial class DapperExtensions
    {
        public static async Task<int> AddAsync<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var type = typeof(T);
            var adapter = GetSqlAdapter(connection);
            var result = await connection.ExecuteAsync(adapter.GetInsertSql(type), persistentObject, transaction,
                commandTimeout,
                commandType);
//            var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
//            foreach (var keyValuePair in typeMapInfo.PropertyTableDict)
//                await AddRangeAsync(connection,
//                    (IEnumerable) TypeMapInfoHelper.GetPropertyTableValue(persistentObject, keyValuePair.Key),
//                    keyValuePair.Key.PropertyType.GenericTypeArguments.FirstOrDefault(),
//                    transaction,
//                    commandTimeout,
//                    commandType);

            return result;
        }

        public static async Task<int> AddRangeAsync<T>(this IDbConnection connection, IList<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await AddRangeAsync(connection, persistentObjects, typeof(T),
                transaction, commandTimeout, commandType);
        }

        public static async Task<int> AddRangeAsync(this IDbConnection connection, IEnumerable persistentObjects,
            Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var result = await connection.ExecuteAsync(adapter.GetInsertSql(type), persistentObjects, transaction,
                commandTimeout,
                commandType);
//            var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
//            foreach (var persistentObject in persistentObjects)
//            foreach (var keyValuePair in typeMapInfo.PropertyTableDict)
//                await AddRangeAsync(connection,
//                    (IEnumerable) TypeMapInfoHelper.GetPropertyTableValue(persistentObject, keyValuePair.Key),
//                    keyValuePair.Key.PropertyType.GenericTypeArguments.FirstOrDefault(),
//                    transaction,
//                    commandTimeout,
//                    commandType);
            return result;
        }

        public static async Task<int> RemoveAsync<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var id = TypeMapInfoHelper.GetIdValue(persistentObject);
            return await RemoveAsync<T>(connection, id, transaction, commandTimeout, commandType);
        }

        public static async Task<int> RemoveAsync<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.ExecuteAsync(
                adapter.GetDeleteSql(typeof(T), CriteriaType.SingleId),
                new {Id = id},
                transaction, commandTimeout, commandType);
        }

        public static async Task<int> RemoveAllAsync<T>(this IDbConnection connection, IList<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.ExecuteAsync(adapter.GetDeleteSql(typeof(T), CriteriaType.SingleId),
                persistentObjects,
                transaction,
                commandTimeout,
                commandType);
        }

        public static async Task<int> RemoveAllAsync<T>(this IDbConnection connection, object ids,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.ExecuteAsync(adapter.GetDeleteSql(typeof(T), CriteriaType.MultiId),
                new {Ids = (IEnumerable) ids}, transaction, commandTimeout, commandType);
        }

        public static async Task<int> RemoveAllAsync<T>(this IDbConnection connection,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.ExecuteAsync(adapter.GetDeleteSql(typeof(T), CriteriaType.None), null, transaction,
                commandTimeout,
                commandType);
        }

        public static async Task<int> UpdateAsync<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.ExecuteAsync(adapter.GetUpdateSql(typeof(T)), persistentObject, transaction,
                commandTimeout,
                commandType);
        }

        public static async Task<int> UpdateAllAsync<T>(this IDbConnection connection, IList<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.ExecuteAsync(adapter.GetUpdateSql(typeof(T)),
                persistentObjects, transaction, commandTimeout, commandType);
        }

        public static async Task<T> FirstOrDefaultAsync<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.QueryFirstOrDefaultAsync<T>(adapter.GetSelectSql(typeof(T), CriteriaType.SingleId),
                new {Id = id},
                transaction, commandTimeout, commandType);
        }

        public static async Task<IList<T>> GetAsync<T>(this IDbConnection connection, object ids,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(T), CriteriaType.MultiId);
            return (await connection.QueryAsync<T>(sql,
                new {Ids = (IEnumerable) ids},
                transaction, commandTimeout, commandType)).ToList();
        }

        public static async Task<IList<T>> GetAllAsync<T>(this IDbConnection connection,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return (await connection.QueryAsync<T>(adapter.GetSelectSql(typeof(T), CriteriaType.None), null,
                transaction,
                commandTimeout, commandType)).ToList();
        }
    }
}