/* License: http://www.apache.org/licenses/LICENSE-2.0 */

using Zaabee.Dapper.Lambda.ValueObjects;

namespace Zaabee.Dapper.Lambda.Builder
{
    /// <summary>
    /// Implements the SQL building for JOIN, ORDER BY, SELECT, and GROUP BY
    /// </summary>
    partial class SqlQueryBuilder
    {
        public void Join(string originalTableName, string joinTableName, string leftField, string rightField)
        {
            var joinString =
                $"JOIN {Adapter.Table(joinTableName)} ON {Adapter.Field(originalTableName, leftField)} = {Adapter.Field(joinTableName, rightField)}";
            TableNames.Add(joinTableName);
            JoinExpressions.Add(joinString);
            SplitColumns.Add(rightField);
        }

        public void OrderBy(string tableName, string fieldName, bool desc = false)
        {
            var order = Adapter.Field(tableName, fieldName);
            if (desc)
                order += " DESC";

            OrderByList.Add(order);
        }

        public void Select(string tableName)
        {
            var selectionString = $"{Adapter.Table(tableName)}.*";
            SelectionList.Add(selectionString);
        }

        public void Select(string tableName, string fieldName)
        {
            SelectionList.Add(Adapter.Field(tableName, fieldName));
        }

        public void Select(string tableName, string fieldName, SelectFunction selectFunction)
        {
            var selectionString = $"{selectFunction.ToString()}({Adapter.Field(tableName, fieldName)})";
            SelectionList.Add(selectionString);
        }

        public void GroupBy(string tableName, string fieldName)
        {
            GroupByList.Add(Adapter.Field(tableName, fieldName));
        }
    }
}