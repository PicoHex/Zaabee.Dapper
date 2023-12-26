namespace Zaabee.Dapper.Extensions.Adapters;

internal class PostgresAdapter : DefaultSqlAdapter
{
    protected override string FormatTableName(string tableName) => $"\"{tableName}\"";

    protected override string FormatColumnName(string columnName) => $"\"{columnName}\"";

    protected override string CriteriaTypeStringParse(
        TypeMapInfo typeMapInfo,
        CriteriaType criteriaType
    )
    {
        return criteriaType switch
        {
            CriteriaType.None => string.Empty,
            CriteriaType.SingleId => $"WHERE \"{typeMapInfo.IdColumnName}\" = @Id",
            CriteriaType.MultiId => $"WHERE \"{typeMapInfo.IdColumnName}\" = ANY(@Ids)",
            _ => throw new ArgumentOutOfRangeException(nameof(criteriaType), criteriaType, null)
        };
    }
}
