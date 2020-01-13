using System;
using Zaabee.Dapper.Extensions.Enums;

namespace Zaabee.Dapper.Extensions.Adapters
{
    internal class PostgresAdapter : DefaultSqlAdapter
    {
        protected override string FormatColumnName(string columnName)
        {
            return $"\"{columnName}\"";
        }

        protected override string CriteriaTypeStringParse(TypeMapInfo typeMapInfo, CriteriaType criteriaType)
        {
            switch (criteriaType)
            {
                case CriteriaType.None:
                    return string.Empty;
                case CriteriaType.SingleId:
                    return $"WHERE {typeMapInfo.TableName}.{typeMapInfo.IdColumnName} = @Id";
                case CriteriaType.MultiId:
                    return $"WHERE {typeMapInfo.TableName}.{typeMapInfo.IdColumnName} = ANY(@Ids)";
                default:
                    throw new ArgumentOutOfRangeException(nameof(criteriaType), criteriaType, null);
            }
        }
    }
}