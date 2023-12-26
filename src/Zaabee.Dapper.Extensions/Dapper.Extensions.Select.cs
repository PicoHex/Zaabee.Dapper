namespace Zaabee.Dapper.Extensions;

public static partial class DapperExtensions
{
    public static T FirstOrDefault<T>(
        this IDbConnection connection,
        object id,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        var adapter = GetSqlAdapter(connection);
        return connection.QueryFirstOrDefault<T>(
            adapter.GetSelectSql(typeof(T), CriteriaType.SingleId),
            new { Id = id },
            transaction,
            commandTimeout,
            commandType
        );
    }

    public static IList<T> Get<T>(
        this IDbConnection connection,
        IEnumerable ids,
        IDbTransaction? transaction = null,
        bool buffered = true,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        var adapter = GetSqlAdapter(connection);
        var sql = adapter.GetSelectSql(typeof(T), CriteriaType.MultiId);
        return connection
            .Query<T>(sql, new { Ids = ids }, transaction, buffered, commandTimeout, commandType)
            .ToList();
    }

    public static IList<T> Take<T>(
        this IDbConnection connection,
        int count,
        IDbTransaction? transaction = null,
        bool buffered = true,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        var adapter = GetSqlAdapter(connection);
        var type = typeof(T);
        var sb = new StringBuilder(adapter.GetSelectSql(type, CriteriaType.None).Trim());
        sb.Insert(6, $" TOP {count}")
            .Append($" ORDER BY {TypeMapInfoHelper.GetTypeMapInfo(type).IdColumnName}");
        return connection
            .Query<T>(sb.ToString(), null, transaction, buffered, commandTimeout, commandType)
            .ToList();
    }

    public static IList<T> GetAll<T>(
        this IDbConnection connection,
        IDbTransaction? transaction = null,
        bool buffered = true,
        int? commandTimeout = null,
        CommandType? commandType = null
    )
    {
        var adapter = GetSqlAdapter(connection);
        return connection
            .Query<T>(
                adapter.GetSelectSql(typeof(T), CriteriaType.None),
                null,
                transaction,
                buffered,
                commandTimeout,
                commandType
            )
            .ToList();
    }
}
