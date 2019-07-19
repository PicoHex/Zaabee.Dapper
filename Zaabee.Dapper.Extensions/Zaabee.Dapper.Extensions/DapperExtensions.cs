using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Zaabee.Dapper.Extensions.Adapters;

namespace Zaabee.Dapper.Extensions
{
    public static class DapperExtensions
    {
        private static readonly Dictionary<string, ISqlAdapter> AdapterDictionary =
            new Dictionary<string, ISqlAdapter>
            {
                ["sqlconnection"] = new SqlServerAdapter(),
                ["npgsqlconnection"] = new PostgresAdapter(),
                ["mysqlconnection"] = new MySqlAdapter()
            };

        private static ISqlAdapter GetSqlAdapter(IDbConnection dbConnection)
        {
            var connName = dbConnection.GetType().Name.ToLower();
            return AdapterDictionary.ContainsKey(connName) ? AdapterDictionary[connName] : new DefaultSqlAdapter();
        }

        #region sync

        public static int Add<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetInsertSql(typeof(T)), persistentObject, transaction, commandTimeout,
                commandType);
        }

        public static int AddRange<T>(this IDbConnection connection, IList<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetInsertSql(typeof(T)), persistentObjects, transaction, commandTimeout,
                commandType);
        }

        public static int Remove<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Remove<T>(connection, TypeMapInfoHelper.GetIdValue(persistentObject), transaction, commandTimeout,
                commandType);
        }

        public static int Remove<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(
                adapter.GetDeleteSql(typeof(T), CriteriaType.SingleId),
                new {Id = id},
                transaction, commandTimeout, commandType);
        }

        public static int RemoveAll<T>(this IDbConnection connection, IList<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetDeleteSql(typeof(T), CriteriaType.SingleId),
                persistentObjects,
                transaction,
                commandTimeout,
                commandType);
        }

        public static int RemoveAll<T>(this IDbConnection connection, object ids,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetDeleteSql(typeof(T), CriteriaType.MultiIds),
                new {Ids = (IEnumerable)ids},
                transaction,
                commandTimeout,
                commandType);
        }

        public static int RemoveAll<T>(this IDbConnection connection,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetDeleteSql(typeof(T), CriteriaType.None), null, transaction,
                commandTimeout,
                commandType);
        }

        public static int Update<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetUpdateSql(typeof(T)), persistentObject, transaction, commandTimeout,
                commandType);
        }

        public static int UpdateAll<T>(this IDbConnection connection, IList<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetUpdateSql(typeof(T)), persistentObjects, transaction, commandTimeout,
                commandType);
        }

        public static T Single<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.QuerySingle<T>(adapter.GetSelectSql(typeof(T), CriteriaType.SingleId), new {Id = id},
                transaction,
                commandTimeout, commandType);
        }

        public static T First<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.QueryFirst<T>(adapter.GetSelectSql(typeof(T), CriteriaType.SingleId), new {Id = id},
                transaction,
                commandTimeout, commandType);
        }

        public static T SingleOrDefault<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.QuerySingleOrDefault<T>(adapter.GetSelectSql(typeof(T), CriteriaType.SingleId),
                new {Id = id},
                transaction, commandTimeout, commandType);
        }

        public static T FirstOrDefault<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.QueryFirstOrDefault<T>(adapter.GetSelectSql(typeof(T), CriteriaType.SingleId),
                new {Id = id},
                transaction, commandTimeout, commandType);
        }

        public static IList<T> Query<T>(this IDbConnection connection, object ids,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(T), CriteriaType.MultiIds);
            return connection.Query<T>(sql,
                new {Ids = (IEnumerable)ids},
                transaction, buffered, commandTimeout, commandType).ToList();
        }

        public static IList<T> Query<T>(this IDbConnection connection,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Query<T>(adapter.GetSelectSql(typeof(T), CriteriaType.None), null, transaction, buffered,
                commandTimeout, commandType).ToList();
        }

        #endregion

        #region async

        public static async Task<int> AddAsync<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.ExecuteAsync(adapter.GetInsertSql(typeof(T)), persistentObject, transaction,
                commandTimeout,
                commandType);
        }

        public static async Task<int> AddRangeAsync<T>(this IDbConnection connection, IList<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.ExecuteAsync(adapter.GetInsertSql(typeof(T)), persistentObjects, transaction,
                commandTimeout, commandType);
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
            return await connection.ExecuteAsync(adapter.GetDeleteSql(typeof(T), CriteriaType.MultiIds),
                new {Ids = (IEnumerable)ids}, transaction, commandTimeout, commandType);
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

        public static async Task<T> SingleAsync<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.QuerySingleAsync<T>(adapter.GetSelectSql(typeof(T), CriteriaType.SingleId),
                new {Id = id},
                transaction, commandTimeout, commandType);
        }

        public static async Task<T> FirstAsync<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.QueryFirstAsync<T>(adapter.GetSelectSql(typeof(T), CriteriaType.SingleId),
                new {Id = id},
                transaction, commandTimeout, commandType);
        }

        public static async Task<T> SingleOrDefaultAsync<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.QuerySingleOrDefaultAsync<T>(adapter.GetSelectSql(typeof(T), CriteriaType.SingleId),
                new {Id = id}, transaction, commandTimeout, commandType);
        }

        public static async Task<T> FirstOrDefaultAsync<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.QueryFirstOrDefaultAsync<T>(adapter.GetSelectSql(typeof(T), CriteriaType.SingleId),
                new {Id = id}, transaction, commandTimeout, commandType);
        }

        public static async Task<IList<T>> QueryAsync<T>(this IDbConnection connection, object ids,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return (await connection.QueryAsync<T>(adapter.GetSelectSql(typeof(T), CriteriaType.MultiIds),
                new {Ids = (IEnumerable)ids}, transaction, commandTimeout, commandType)).ToList();
        }

        public static async Task<IList<T>> QueryAsync<T>(this IDbConnection connection,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return (await connection.QueryAsync<T>(adapter.GetSelectSql(typeof(T), CriteriaType.None), null,
                transaction,
                commandTimeout, commandType)).ToList();
        }

        #endregion
    }
}