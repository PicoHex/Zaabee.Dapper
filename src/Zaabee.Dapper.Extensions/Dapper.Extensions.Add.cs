using System;
using System.Collections;
using System.Data;
using Dapper;

namespace Zaabee.Dapper.Extensions
{
    public static partial class DapperExtensions
    {
        public static int Add<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            Add(connection, persistentObject, typeof(T), transaction, commandTimeout, commandType);

        public static int Add(this IDbConnection connection, object persistentObject, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetInsertSql(type), persistentObject, transaction, commandTimeout,
                commandType);
        }

        public static int AddRange<T>(this IDbConnection connection, IEnumerable persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
            AddRange(connection, persistentObjects, typeof(T), transaction, commandTimeout, commandType);

        public static int AddRange(this IDbConnection connection, IEnumerable persistentObjects, Type type,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return connection.Execute(adapter.GetInsertSql(type), persistentObjects, transaction, commandTimeout,
                commandType);
        }
    }
}