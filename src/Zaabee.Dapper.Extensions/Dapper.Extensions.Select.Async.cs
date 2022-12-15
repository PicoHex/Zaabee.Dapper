namespace Zaabee.Dapper.Extensions;

public static partial class DapperExtensions
{
    public static async Task<T> FirstOrDefaultAsync<T>(this IDbConnection connection, object id,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var adapter = GetSqlAdapter(connection);
        return await connection.QueryFirstOrDefaultAsync<T>(adapter.GetSelectSql(typeof(T), CriteriaType.SingleId),
            new {Id = id}, transaction, commandTimeout, commandType);
    }

    public static async Task<IList<T>> GetAsync<T>(this IDbConnection connection, object ids,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var adapter = GetSqlAdapter(connection);
        var sql = adapter.GetSelectSql(typeof(T), CriteriaType.MultiId);
        return (await connection.QueryAsync<T>(sql, new {Ids = (IEnumerable) ids},
            transaction, commandTimeout, commandType)).ToList();
    }
        
    public static async Task<IList<T>> TakeAsync<T>(this IDbConnection connection, int count,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var adapter = GetSqlAdapter(connection);
        var type = typeof(T);
        var sb = new StringBuilder(adapter.GetSelectSql(type, CriteriaType.None).Trim());
        sb.Insert(6, $" TOP {count}").Append($" ORDER BY {TypeMapInfoHelper.GetTypeMapInfo(type).IdColumnName}");
        return (await connection.QueryAsync<T>(sb.ToString(), null, transaction, commandTimeout, commandType)).ToList();
    }

    public static async Task<IList<T>> GetAllAsync<T>(this IDbConnection connection,
        IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    {
        var adapter = GetSqlAdapter(connection);
        return (await connection.QueryAsync<T>(adapter.GetSelectSql(typeof(T), CriteriaType.None), null,
            transaction, commandTimeout, commandType)).ToList();
    }
}