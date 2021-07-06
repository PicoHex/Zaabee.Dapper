using System;
using Zaabee.Dapper.Extensions.Enums;

namespace Zaabee.Dapper.Extensions.Adapters
{
    internal interface ISqlAdapter
    {
        string GetInsertSql(Type type);
        string GetDeleteSql(Type type, CriteriaType conditionType);
        string GetUpdateSql(Type type);
        string GetSelectSql(Type type, CriteriaType criteriaType);
    }
}