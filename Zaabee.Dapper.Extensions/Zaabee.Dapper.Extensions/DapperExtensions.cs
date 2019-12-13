using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Zaabee.Dapper.Extensions.Adapters;
using Zaabee.Dapper.Extensions.Enums;

namespace Zaabee.Dapper.Extensions
{
    public partial class DapperExtensions
    {
        private static readonly Dictionary<string, ISqlAdapter> AdapterDictionary =
            new Dictionary<string, ISqlAdapter>
            {
                ["idbconnection"] = new DefaultSqlAdapter(),
                ["sqlconnection"] = new SqlServerAdapter(),
                ["npgsqlconnection"] = new PostgresAdapter(),
                ["mysqlconnection"] = new MySqlAdapter(),
                ["sqliteconnection"] = new SQLiteAdapter()
            };

        private static ISqlAdapter GetSqlAdapter(IDbConnection dbConnection)
        {
            var connName = dbConnection.GetType().Name.ToLower();
            return AdapterDictionary.ContainsKey(connName)
                ? AdapterDictionary[connName]
                : AdapterDictionary.First().Value;
        }

        #region Add

        public static int Add<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            Add(connection, persistentObject, typeof(T), transaction, commandTimeout, commandType);

        public static int Add<T>(this IDbConnection connection, T persistentObject, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var result = connection.Execute(adapter.GetInsertSql(type), persistentObject, transaction, commandTimeout,
                commandType);
            var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
            foreach (var keyValuePair in typeMapInfo.PropertyTableDict)
                AddRange(connection,
                    (IEnumerable) TypeMapInfoHelper.GetPropertyTableValue(persistentObject, keyValuePair.Key),
                    keyValuePair.Key.PropertyType.GenericTypeArguments.FirstOrDefault(),
                    transaction,
                    commandTimeout,
                    commandType);

            return result;
        }

        public static int AddRange<T>(this IDbConnection connection, IEnumerable persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            AddRange(connection, persistentObjects, typeof(T), transaction, commandTimeout, commandType);

        public static int AddRange(this IDbConnection connection, IEnumerable persistentObjects, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var result = connection.Execute(adapter.GetInsertSql(type), persistentObjects, transaction, commandTimeout,
                commandType);
            var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
            foreach (var keyValuePair in typeMapInfo.PropertyTableDict)
            foreach (var persistentObject in persistentObjects)
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

        public static int RemoveByEntity<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            RemoveByEntity(connection, persistentObject, typeof(T), transaction, commandTimeout, commandType);

        public static int RemoveByEntity(this IDbConnection connection, object persistentObject, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(
                adapter.GetDeleteSql(type, CriteriaType.SingleId),
                persistentObject,
                transaction, commandTimeout, commandType);
        }

        public static int RemoveById<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            RemoveById(connection, id, typeof(T), transaction, commandTimeout, commandType);

        public static int RemoveById(this IDbConnection connection, object id, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(
                adapter.GetDeleteSql(type, CriteriaType.SingleId),
                new {Id = id},
                transaction, commandTimeout, commandType);
        }

        public static int RemoveByEntities<T>(this IDbConnection connection, IEnumerable persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            RemoveByEntities(connection, persistentObjects, typeof(T), transaction, commandTimeout, commandType);

        public static int RemoveByEntities(this IDbConnection connection, IEnumerable persistentObjects, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetDeleteSql(type, CriteriaType.SingleId),
                persistentObjects,
                transaction,
                commandTimeout,
                commandType);
        }

        public static int RemoveByIds<T>(this IDbConnection connection, IEnumerable ids,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            RemoveByIds(connection, ids, typeof(T), transaction, commandTimeout, commandType);

        public static int RemoveByIds(this IDbConnection connection, IEnumerable ids, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetDeleteSql(type, CriteriaType.MultiId),
                new {Ids = ids},
                transaction,
                commandTimeout,
                commandType);
        }

        public static int RemoveAll<T>(this IDbConnection connection,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            RemoveAll(connection, typeof(T), transaction, commandTimeout, commandType);

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
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            Update(connection, persistentObject, typeof(T), transaction, commandTimeout, commandType);

        public static int Update(this IDbConnection connection, object persistentObject, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
            foreach (var keyValuePair in typeMapInfo.PropertyTableDict)
            {
                RemoveByEntities(connection,
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

        public static int UpdateAll<T>(this IDbConnection connection, IEnumerable persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            UpdateAll(connection, persistentObjects, typeof(T), transaction, commandTimeout, commandType);

        public static int UpdateAll(this IDbConnection connection, IEnumerable persistentObjects, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
            foreach (var keyValuePair in typeMapInfo.PropertyTableDict)
            foreach (var persistentObject in persistentObjects)
            {
                RemoveByEntities(connection,
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

        #region Select

        #region FirstOrDefault

        public static T FirstOrDefault<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.QueryFirstOrDefault<T>(adapter.GetSelectSql(typeof(T), CriteriaType.SingleId),
                new {Id = id}, transaction, commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> FirstOrDefault<TFirst, TSecond, TReturn>(this IDbConnection connection,
            object id, Func<TFirst, TSecond, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(TFirst), CriteriaType.SingleId);
            return connection.Query(sql, map, new {Id = id}, transaction, buffered, splitOn,
                commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> FirstOrDefault<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            this IDbConnection connection,
            object id, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(TFirst), CriteriaType.SingleId);
            return connection.Query(sql, map, new {Id = id}, transaction, buffered, splitOn,
                commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> FirstOrDefault<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            this IDbConnection connection,
            object id, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(TFirst), CriteriaType.SingleId);
            return connection.Query(sql, map, new {Id = id}, transaction, buffered, splitOn,
                commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> FirstOrDefault<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            this IDbConnection connection,
            object id, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
            IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(TFirst), CriteriaType.SingleId);
            return connection.Query(sql, map, new {Id = id}, transaction, buffered, splitOn,
                commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> FirstOrDefault<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh,
            TReturn>(this IDbConnection connection,
            object id, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
            IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(TFirst), CriteriaType.SingleId);
            return connection.Query(sql, map, new {Id = id}, transaction, buffered, splitOn,
                commandTimeout, commandType);
        }

        #endregion

        #region Get

        public static IList<T> Get<T>(this IDbConnection connection, IEnumerable ids,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(T), CriteriaType.MultiId);
            return connection.Query<T>(sql,
                new {Ids = ids},
                transaction, buffered, commandTimeout, commandType).ToList();
        }

        public static IEnumerable<TReturn> Get<TFirst, TSecond, TReturn>(this IDbConnection connection,
            IEnumerable ids, Func<TFirst, TSecond, TReturn> map,
            IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(TFirst), CriteriaType.MultiId);
            return connection.Query(sql, map, new {Ids = ids}, transaction, buffered, splitOn,
                commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh,
            TReturn>(this IDbConnection connection,
            IEnumerable ids, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
            IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(TFirst), CriteriaType.MultiId);
            return connection.Query(sql, map, new {Ids = ids}, transaction, buffered, splitOn,
                commandTimeout, commandType);
        }

        #endregion

        #region GetAll

        public static IList<T> GetAll<T>(this IDbConnection connection,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Query<T>(adapter.GetSelectSql(typeof(T), CriteriaType.None), null, transaction, buffered,
                commandTimeout, commandType).ToList();
        }

        public static IEnumerable<TReturn> GetAll<TFirst, TSecond, TReturn>(this IDbConnection connection,
            Func<TFirst, TSecond, TReturn> map,
            IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(TFirst), CriteriaType.None);
            return connection.Query(sql, map, null, transaction, buffered, splitOn,
                commandTimeout, commandType);
        }

        public static IEnumerable<TReturn> GetAll<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh,
            TReturn>(this IDbConnection connection,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
            IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(TFirst), CriteriaType.None);
            return connection.Query(sql, map, null, transaction, buffered, splitOn,
                commandTimeout, commandType);
        }

        #endregion

        #endregion
    }
}