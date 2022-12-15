namespace Zaabee.Dapper.Extensions;

public static partial class DapperExtensions
{
    public static int Update<T>(this IDbConnection connection, T persistentObject,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
        Update(connection, persistentObject, typeof(T), transaction, commandTimeout, commandType);

    public static int Update(this IDbConnection connection, object persistentObject, Type type,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var adapter = GetSqlAdapter(connection);
        return connection.Execute(adapter.GetUpdateSql(type), persistentObject, transaction, commandTimeout,
            commandType);
    }

    public static int UpdateAll<T>(this IDbConnection connection, IEnumerable persistentObjects,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
        UpdateAll(connection, persistentObjects, typeof(T), transaction, commandTimeout, commandType);

    public static int UpdateAll(this IDbConnection connection, IEnumerable persistentObjects, Type type,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var adapter = GetSqlAdapter(connection);
        return connection.Execute(adapter.GetUpdateSql(type), persistentObjects, transaction, commandTimeout,
            commandType);
    }
}