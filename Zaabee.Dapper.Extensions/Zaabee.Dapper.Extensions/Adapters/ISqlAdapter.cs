using System;

namespace Zaabee.Dapper.Extensions.Adapters
{
    public interface ISqlAdapter
    {
        string GetInsertSql(Type type);
        string GetDeleteSql(Type type, CriteriaType conditionType);
        string GetUpdateSql(Type type);
        string GetSelectSql(Type type, CriteriaType criteriaType);
    }
}