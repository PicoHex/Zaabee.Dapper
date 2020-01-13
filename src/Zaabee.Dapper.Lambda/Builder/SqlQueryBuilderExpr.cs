/* License: http://www.apache.org/licenses/LICENSE-2.0 */
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zaabee.Dapper.Lambda.Builder
{
    /// <summary>
    /// Implements the expression building for the WHERE statement
    /// </summary>
    partial class SqlQueryBuilder
    {
        public void BeginExpression()
        {
            WhereConditions.Add("(");
        }

        public void EndExpression()
        {
            WhereConditions.Add(")");
        }

        public void And()
        {
            if (WhereConditions.Count > 0)
                WhereConditions.Add(" AND ");
        }

        public void Or()
        {
            if (WhereConditions.Count > 0)
                WhereConditions.Add(" OR ");
        }

        public void Not()
        {
            WhereConditions.Add(" NOT ");
        }

        public void QueryByField(string tableName, string fieldName, string op, object fieldValue)
        {
            var paramId = NextParamId();
            var newCondition = $"{Adapter.Field(tableName, fieldName)} {op} {Adapter.Parameter(paramId)}";

            WhereConditions.Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        public void QueryByFieldLike(string tableName, string fieldName, string fieldValue)
        {
            var paramId = NextParamId();
            var newCondition = $"{Adapter.Field(tableName, fieldName)} LIKE {Adapter.Parameter(paramId)}";

            WhereConditions.Add(newCondition);
            AddParameter(paramId, fieldValue);
        }

        public void QueryByFieldNull(string tableName, string fieldName)
        {
            WhereConditions.Add($"{Adapter.Field(tableName, fieldName)} IS NULL");
        }

        public void QueryByFieldNotNull(string tableName, string fieldName)
        {
            WhereConditions.Add($"{Adapter.Field(tableName, fieldName)} IS NOT NULL");
        }

        public void QueryByFieldComparison(string leftTableName, string leftFieldName, string op,
            string rightTableName, string rightFieldName)
        {
            var newCondition =
                $"{Adapter.Field(leftTableName, leftFieldName)} {op} {Adapter.Field(rightTableName, rightFieldName)}";

            WhereConditions.Add(newCondition);
        }

        public void QueryByIsIn(string tableName, string fieldName, SqlLamBase sqlQuery)
        {
            var innerQuery = sqlQuery.QueryString;
            foreach (var param in sqlQuery.QueryParameters)
            {
                var innerParamKey = "Inner" + param.Key;
                innerQuery = Regex.Replace(innerQuery, param.Key, innerParamKey);
                AddParameter(innerParamKey, param.Value);
            }

            var newCondition = $"{Adapter.Field(tableName, fieldName)} IN ({innerQuery})";

            WhereConditions.Add(newCondition);
        }

        public void QueryByIsIn(string tableName, string fieldName, IEnumerable<object> values)
        {
            var paramIds = values.Select(x =>
            {
                var paramId = NextParamId();
                AddParameter(paramId, x);
                return Adapter.Parameter(paramId);
            });

            var newCondition = $"{Adapter.Field(tableName, fieldName)} IN ({string.Join(",", paramIds)})";
            WhereConditions.Add(newCondition);
        }
    }
}