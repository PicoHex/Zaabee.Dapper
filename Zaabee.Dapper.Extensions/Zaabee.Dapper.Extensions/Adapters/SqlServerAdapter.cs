using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zaabee.Dapper.Extensions.Adapters
{
    public class SqlServerAdapter : DefaultSqlAdapter
    {
        private static readonly ConcurrentDictionary<Type, string> InsertSqlCache =
            new ConcurrentDictionary<Type, string>();

        private static readonly ConcurrentDictionary<Type, Dictionary<CriteriaType, string>> DeleteSqlCache =
            new ConcurrentDictionary<Type, Dictionary<CriteriaType, string>>();

        private static readonly ConcurrentDictionary<Type, string> UpdateSqlDict =
            new ConcurrentDictionary<Type, string>();

        private static readonly ConcurrentDictionary<Type, Dictionary<CriteriaType, string>> SelectSqlCache =
            new ConcurrentDictionary<Type, Dictionary<CriteriaType, string>>();

        public override string GetInsertSql(Type type)
        {
            return InsertSqlCache.GetOrAdd(type, key =>
            {
                var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);

                var columnNames = new List<string> {typeMapInfo.IdColumnName};
                columnNames.AddRange(typeMapInfo.PropertyColumnDict.Select(pair => pair.Key));

                var propertyNames = new List<string> {typeMapInfo.IdPropertyInfo.Name};
                propertyNames.AddRange(typeMapInfo.PropertyColumnDict.Select(pair => pair.Value.Name));

                var intoString = string.Join(",", columnNames);
                var valueString = string.Join(",", propertyNames.Select(propertyName => $"@{propertyName}"));
                return $"INSERT INTO {typeMapInfo.TableName} ({intoString}) VALUES ({valueString})";
            });
        }

        public override string GetDeleteSql(Type type, CriteriaType conditionType)
        {
            var sqls = DeleteSqlCache.GetOrAdd(type, typeKey =>
            {
                var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
                var fromString = $"DELETE FROM {typeMapInfo.TableName}";
                var whereEqualIdString = $"WHERE {typeMapInfo.IdColumnName} = @Id";
                var whereAnyIdsString = $"WHERE {typeMapInfo.IdColumnName} IN @Ids";
                return new Dictionary<CriteriaType, string>
                {
                    {
                        CriteriaType.SingleId,
                        $"{fromString} {whereEqualIdString}"
                    },
                    {
                        CriteriaType.MultiIds,
                        $"{fromString} {whereAnyIdsString}"
                    },
                    {
                        CriteriaType.None,
                        $"{fromString}"
                    }
                };
            });

            return sqls[conditionType];
        }

        public override string GetUpdateSql(Type type)
        {
            return UpdateSqlDict.GetOrAdd(type,
                key =>
                {
                    var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
                    var setSql = string.Join(",",
                        typeMapInfo.PropertyColumnDict.Select(pair => $"{pair.Key} = @{pair.Value.Name}"));
                    return
                        $"UPDATE {typeMapInfo.TableName} SET {setSql} WHERE {typeMapInfo.IdColumnName} = @{typeMapInfo.IdPropertyInfo.Name}";
                });
        }

        public override string GetSelectSql(Type type, CriteriaType criteriaType)
        {
            var typeSql = SelectSqlCache.GetOrAdd(type, key =>
            {
                var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
                var propertyInfoDict = new Dictionary<string, PropertyInfo>
                    {{typeMapInfo.IdColumnName, typeMapInfo.IdPropertyInfo}};
                foreach (var pair in typeMapInfo.PropertyColumnDict)
                    propertyInfoDict.Add(pair.Key, pair.Value);
                var selectAllFieldsString =
                    $"SELECT {string.Join(",", propertyInfoDict.Select(pair => $"{pair.Key} AS {pair.Value.Name}"))}";
                var fromString = $"FROM {typeMapInfo.TableName}";
                var whereEqualIdString = $"WHERE {typeMapInfo.IdColumnName} = @Id";
                var whereAnyIdsString = $"WHERE {typeMapInfo.IdColumnName} IN @Ids";

                return new Dictionary<CriteriaType, string>
                {
                    {
                        CriteriaType.SingleId,
                        $"{selectAllFieldsString} {fromString} {whereEqualIdString}"
                    },
                    {
                        CriteriaType.MultiIds,
                        $"{selectAllFieldsString} {fromString} {whereAnyIdsString}"
                    },
                    {
                        CriteriaType.None,
                        $"{selectAllFieldsString} {fromString}"
                    }
                };
            });

            return typeSql[criteriaType];
        }
    }
}