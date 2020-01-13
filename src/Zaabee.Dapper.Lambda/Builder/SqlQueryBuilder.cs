/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using Zaabee.Dapper.Lambda.Adapter;

namespace Zaabee.Dapper.Lambda.Builder
{
    /// <summary>
    /// Implements the whole SQL building logic. Continually adds and stores the SQL parts as the requests come. 
    /// When requested to return the QueryString, the parts are combined and returned as a single query string.
    /// The query parameters are stored in a dictionary implemented by an ExpandoObject that can be requested by QueryParameters.
    /// </summary>
    public partial class SqlQueryBuilder
    {
        private ISqlAdapter Adapter { get; set; }

        private const string ParameterPrefix = "Param";

        public List<string> TableNames { get; } = new List<string>();

        public List<string> JoinExpressions { get; } = new List<string>();

        public List<string> SelectionList { get; } = new List<string>();

        public List<string> WhereConditions { get; } = new List<string>();

        public List<string> OrderByList { get; } = new List<string>();

        public List<string> GroupByList { get; } = new List<string>();

        public List<string> HavingConditions { get; } = new List<string>();

        public List<string> SplitColumns { get; } = new List<string>();

        public int CurrentParamIndex { get; private set; }

        private string Source
        {
            get
            {
                var joinExpression = string.Join(" ", JoinExpressions);
                return $"{Adapter.Table(TableNames.First())} {joinExpression}";
            }
        }

        private string Selection => SelectionList.Count == 0
            ? $"{Adapter.Table(TableNames.First())}.*"
            : string.Join(", ", SelectionList);

        private string Conditions
        {
            get
            {
                if (WhereConditions.Count == 0)
                    return "";
                return "WHERE " + string.Join("", WhereConditions);
            }
        }

        private string Order
        {
            get
            {
                if (OrderByList.Count == 0)
                    return "";
                return "ORDER BY " + string.Join(", ", OrderByList);
            }
        }

        private string Grouping
        {
            get
            {
                if (GroupByList.Count == 0)
                    return "";
                return "GROUP BY " + string.Join(", ", GroupByList);
            }
        }

        private string Having
        {
            get
            {
                if (HavingConditions.Count == 0)
                    return "";
                return "HAVING " + string.Join(" ", HavingConditions);
            }
        }

        public IDictionary<string, object> Parameters { get; private set; }

        public string QueryString => Adapter.QueryString(Selection, Source, Conditions, Grouping, Having, Order);

        public string QueryStringPage(int pageSize, int? pageNumber = null)
        {
            if (!pageNumber.HasValue) return Adapter.QueryStringPage(Source, Selection, Conditions, Order, pageSize);
            if (OrderByList.Count == 0)
                throw new Exception("Pagination requires the ORDER BY statement to be specified");

            return Adapter.QueryStringPage(Source, Selection, Conditions, Order, pageSize, pageNumber.Value);
        }

        internal SqlQueryBuilder(string tableName, ISqlAdapter adapter)
        {
            TableNames.Add(tableName);
            Adapter = adapter;
            Parameters = new ExpandoObject();
            CurrentParamIndex = 0;
        }

        #region helpers

        private string NextParamId()
        {
            ++CurrentParamIndex;
            return ParameterPrefix + CurrentParamIndex.ToString(CultureInfo.InvariantCulture);
        }

        private void AddParameter(string key, object value)
        {
            if (!Parameters.ContainsKey(key))
                Parameters.Add(key, value);
        }

        #endregion
    }
}