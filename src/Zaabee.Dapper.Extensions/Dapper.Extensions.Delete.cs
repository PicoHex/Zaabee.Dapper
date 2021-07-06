using System;
using System.Collections;
using System.Data;
using Dapper;
using Zaabee.Dapper.Extensions.Enums;

namespace Zaabee.Dapper.Extensions
{
    public static partial class DapperExtensions
    {
        public static int DeleteByEntity<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            DeleteByEntity(connection, persistentObject, typeof(T), transaction, commandTimeout, commandType);

        public static int DeleteByEntity(this IDbConnection connection, object persistentObject, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(
                adapter.GetDeleteSql(type, CriteriaType.SingleId), persistentObject, transaction, commandTimeout,
                commandType);
        }

        public static int DeleteById<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            DeleteById(connection, id, typeof(T), transaction, commandTimeout, commandType);

        public static int DeleteById(this IDbConnection connection, object id, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetDeleteSql(type, CriteriaType.SingleId), new {Id = id}, transaction,
                commandTimeout, commandType);
        }

        public static int DeleteByEntities<T>(this IDbConnection connection, IEnumerable persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            DeleteByEntities(connection, persistentObjects, typeof(T), transaction, commandTimeout, commandType);

        public static int DeleteByEntities(this IDbConnection connection, IEnumerable persistentObjects, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetDeleteSql(type, CriteriaType.SingleId),
                persistentObjects, transaction, commandTimeout, commandType);
        }

        public static int DeleteByIds<T>(this IDbConnection connection, IEnumerable ids,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            DeleteByIds(connection, ids, typeof(T), transaction, commandTimeout, commandType);

        public static int DeleteByIds(this IDbConnection connection, IEnumerable ids, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetDeleteSql(type, CriteriaType.MultiId),
                new {Ids = ids}, transaction, commandTimeout, commandType);
        }

        public static int DeleteAll<T>(this IDbConnection connection,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            DeleteAll(connection, typeof(T), transaction, commandTimeout, commandType);

        public static int DeleteAll(this IDbConnection connection, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetDeleteSql(type, CriteriaType.None), null, transaction,
                commandTimeout, commandType);
        }
    }
}