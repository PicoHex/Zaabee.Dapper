namespace Zaabee.Dapper.Extensions;

public static partial class DapperExtensions
{
    public static Task<int> DeleteByEntityAsync<T>(this IDbConnection connection, T persistentObject,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
        DeleteByEntityAsync(connection, persistentObject, typeof(T), transaction, commandTimeout, commandType);
        
    public static Task<int> DeleteByEntityAsync(this IDbConnection connection, object persistentObject, Type type,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var id = TypeMapInfoHelper.GetIdValue(persistentObject);
        return DeleteByIdAsync(connection, id, type, transaction, commandTimeout, commandType);
    }

    public static Task<int> DeleteByIdAsync<T>(this IDbConnection connection, object id,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null) =>
        DeleteByIdAsync(connection, id, typeof(T), transaction, commandTimeout, commandType);

    public static Task<int> DeleteByIdAsync(this IDbConnection connection, object id, Type type,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var adapter = GetSqlAdapter(connection);
        return connection.ExecuteAsync(adapter.GetDeleteSql(type, CriteriaType.SingleId), new {Id = id}, transaction,
            commandTimeout, commandType);
    }

    public static Task<int> DeleteByEntitiesAsync<T>(this IDbConnection connection, IList<T> persistentObjects,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var adapter = GetSqlAdapter(connection);
        return connection.ExecuteAsync(adapter.GetDeleteSql(typeof(T), CriteriaType.MultiId),
            persistentObjects, transaction, commandTimeout, commandType);
    }

    public static Task<int> DeleteByIdsAsync<T>(this IDbConnection connection, object ids,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var adapter = GetSqlAdapter(connection);
        return connection.ExecuteAsync(adapter.GetDeleteSql(typeof(T), CriteriaType.MultiId),
            new {Ids = (IEnumerable) ids}, transaction, commandTimeout, commandType);
    }

    public static Task<int> DeleteAllAsync<T>(this IDbConnection connection,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var adapter = GetSqlAdapter(connection);
        return connection.ExecuteAsync(adapter.GetDeleteSql(typeof(T), CriteriaType.None), null, transaction,
            commandTimeout, commandType);
    }
}