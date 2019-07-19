using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Zaabee.Dapper.Extensions.Adapters;

namespace Zaabee.Dapper.Extensions
{
    public partial class DapperExtensions
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

        #region Add

        public static int Add<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var type = typeof(T);
            var adapter = GetSqlAdapter(connection);
            var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
            var result = connection.Execute(adapter.GetInsertSql(type), persistentObject, transaction, commandTimeout,
                commandType);
            foreach (var keyValuePair in typeMapInfo.PropertyTableDict)
                AddRange(connection,
                    (IEnumerable) TypeMapInfoHelper.GetPropertyTableValue(persistentObject, keyValuePair.Key),
                    keyValuePair.Key.PropertyType.GenericTypeArguments.FirstOrDefault(),
                    transaction,
                    commandTimeout,
                    commandType);

            return result;
        }

        public static int AddRange<T>(this IDbConnection connection, IList<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return AddRange(connection, persistentObjects, typeof(T),
                transaction, commandTimeout, commandType);
        }

        public static int AddRange(this IDbConnection connection, IEnumerable persistentObjects, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var result = connection.Execute(adapter.GetInsertSql(type), persistentObjects, transaction, commandTimeout,
                commandType);
            var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
            foreach (var persistentObject in persistentObjects)
            foreach (var keyValuePair in typeMapInfo.PropertyTableDict)
                AddRange(connection,
                    (IEnumerable) TypeMapInfoHelper.GetPropertyTableValue(persistentObject, keyValuePair.Key),
                    keyValuePair.Key.PropertyType.GenericTypeArguments.FirstOrDefault(),
                    transaction,
                    commandTimeout,
                    commandType);
            return result;
        }

        #endregion

        #region Remove

        public static int Remove<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(typeof(T));
            foreach (var keyValuePair in typeMapInfo.PropertyTableDict)
                RemoveAll(connection,
                    (IEnumerable) TypeMapInfoHelper.GetPropertyTableValue(persistentObject, keyValuePair.Key),
                    keyValuePair.Key.PropertyType.GenericTypeArguments.FirstOrDefault(),
                    transaction,
                    commandTimeout,
                    commandType);

            return Remove<T>(connection, TypeMapInfoHelper.GetIdValue(persistentObject), transaction, commandTimeout,
                commandType);
        }

        public static int Remove<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Remove(connection, id, typeof(T), transaction, commandTimeout, commandType);
        }

        public static int Remove(this IDbConnection connection, object id, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(
                adapter.GetDeleteSql(type, CriteriaType.SingleId),
                new {Id = id},
                transaction, commandTimeout, commandType);
        }

        public static int RemoveAll<T>(this IDbConnection connection, IList<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return RemoveAll(connection, persistentObjects, typeof(T), transaction, commandTimeout, commandType);
        }

        public static int RemoveAll(this IDbConnection connection, IEnumerable persistentObjects, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
            foreach (var persistentObject in persistentObjects)
            foreach (var keyValuePair in typeMapInfo.PropertyTableDict)
                RemoveAll(connection,
                    (IEnumerable) TypeMapInfoHelper.GetPropertyTableValue(persistentObject, keyValuePair.Key),
                    keyValuePair.Key.PropertyType.GenericTypeArguments.FirstOrDefault(),
                    transaction,
                    commandTimeout,
                    commandType);
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetDeleteSql(type, CriteriaType.SingleId),
                persistentObjects,
                transaction,
                commandTimeout,
                commandType);
        }

        public static int RemoveAll<T>(this IDbConnection connection, object ids,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return RemoveAll(connection, ids, typeof(T), transaction, commandTimeout, commandType);
        }

        public static int RemoveAll(this IDbConnection connection, object ids, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetDeleteSql(type, CriteriaType.MultiIds),
                new {Ids = (IEnumerable) ids},
                transaction,
                commandTimeout,
                commandType);
        }

        public static int RemoveAll<T>(this IDbConnection connection,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return RemoveAll(connection, typeof(T), transaction, commandTimeout, commandType);
        }

        public static int RemoveAll(this IDbConnection connection, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetDeleteSql(type, CriteriaType.None), null, transaction,
                commandTimeout,
                commandType);
        }

        #endregion

        #region Update

        public static int Update<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Update(connection, persistentObject, typeof(T), transaction, commandTimeout, commandType);
        }

        public static int Update(this IDbConnection connection, object persistentObject, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
            foreach (var keyValuePair in typeMapInfo.PropertyTableDict)
            {
                RemoveAll(connection,
                    (IEnumerable) TypeMapInfoHelper.GetPropertyTableValue(persistentObject, keyValuePair.Key),
                    keyValuePair.Key.PropertyType.GenericTypeArguments.FirstOrDefault(),
                    transaction,
                    commandTimeout,
                    commandType);
                AddRange(connection,
                    (IEnumerable) TypeMapInfoHelper.GetPropertyTableValue(persistentObject, keyValuePair.Key),
                    keyValuePair.Key.PropertyType.GenericTypeArguments.FirstOrDefault(),
                    transaction,
                    commandTimeout,
                    commandType);
            }
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetUpdateSql(type), persistentObject, transaction, commandTimeout,
                commandType);
        }

        public static int UpdateAll<T>(this IDbConnection connection, IList<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return UpdateAll(connection, persistentObjects, typeof(T), transaction, commandTimeout, commandType);
        }

        public static int UpdateAll(this IDbConnection connection, IEnumerable persistentObjects, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
            foreach (var persistentObject in persistentObjects)
            foreach (var keyValuePair in typeMapInfo.PropertyTableDict)
            {
                RemoveAll(connection,
                    (IEnumerable) TypeMapInfoHelper.GetPropertyTableValue(persistentObject, keyValuePair.Key),
                    keyValuePair.Key.PropertyType.GenericTypeArguments.FirstOrDefault(),
                    transaction,
                    commandTimeout,
                    commandType);
                AddRange(connection,
                    (IEnumerable) TypeMapInfoHelper.GetPropertyTableValue(persistentObject, keyValuePair.Key),
                    keyValuePair.Key.PropertyType.GenericTypeArguments.FirstOrDefault(),
                    transaction,
                    commandTimeout,
                    commandType);
            }

            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetUpdateSql(type), persistentObjects, transaction, commandTimeout,
                commandType);
        }

        #endregion

        public static T FirstOrDefault<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var result = connection.QueryFirstOrDefault<T>(adapter.GetSelectSql(typeof(T), CriteriaType.SingleId),
                new {Id = id},
                transaction, commandTimeout, commandType);
            return result;
        }

        public static IList<T> Query<T>(this IDbConnection connection, object ids,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(T), CriteriaType.MultiIds);
            return connection.Query<T>(sql,
                new {Ids = (IEnumerable) ids},
                transaction, buffered, commandTimeout, commandType).ToList();
        }

        public static IList<T> GetAll<T>(this IDbConnection connection,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Query<T>(adapter.GetSelectSql(typeof(T), CriteriaType.None), null, transaction, buffered,
                commandTimeout, commandType).ToList();
        }
    }
}