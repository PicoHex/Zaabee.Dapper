using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Zaabee.Dapper.Extensions.Enums;

namespace Zaabee.Dapper.Extensions
{
    public static partial class DapperExtensions
    {
        public static async Task<T> FirstOrDefaultAsync<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return await connection.QueryFirstOrDefaultAsync<T>(adapter.GetSelectSql(typeof(T), CriteriaType.SingleId),
                new {Id = id}, transaction, commandTimeout, commandType);
        }

        public static async Task<IList<T>> GetAsync<T>(this IDbConnection connection, object ids,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var sql = adapter.GetSelectSql(typeof(T), CriteriaType.MultiId);
            return (await connection.QueryAsync<T>(sql, new {Ids = (IEnumerable) ids},
                transaction, commandTimeout, commandType)).ToList();
        }
        
        public static async Task<IList<T>> TakeAsync<T>(this IDbConnection connection, int count,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            var type = typeof(T);
            var sql = adapter.GetSelectSql(type, CriteriaType.None);
            var sb = new StringBuilder(sql.Trim());
            sb.Insert(6, $" TOP {count}").Append($" ORDER BY {TypeMapInfoHelper.GetTypeMapInfo(type).IdColumnName}");
            return (await connection.QueryAsync<T>(sql, null, transaction, commandTimeout, commandType)).ToList();
        }

        public static async Task<IList<T>> GetAllAsync<T>(this IDbConnection connection,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var adapter = GetSqlAdapter(connection);
            return (await connection.QueryAsync<T>(adapter.GetSelectSql(typeof(T), CriteriaType.None), null,
                transaction, commandTimeout, commandType)).ToList();
        }
    }
}