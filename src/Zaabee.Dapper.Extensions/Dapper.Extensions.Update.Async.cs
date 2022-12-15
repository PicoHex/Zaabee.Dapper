namespace Zaabee.Dapper.Extensions;

public static partial class DapperExtensions
{
    public static Task<int> UpdateAsync<T>(this IDbConnection connection, T persistentObject,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var adapter = GetSqlAdapter(connection);
        return connection.ExecuteAsync(adapter.GetUpdateSql(typeof(T)), persistentObject, transaction,
            commandTimeout, commandType);
    }

    public static Task<int> UpdateAllAsync<T>(this IDbConnection connection, IList<T> persistentObjects,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var adapter = GetSqlAdapter(connection);
        return connection.ExecuteAsync(adapter.GetUpdateSql(typeof(T)),
            persistentObjects, transaction, commandTimeout, commandType);
    }
}