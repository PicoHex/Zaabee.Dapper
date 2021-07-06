using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Zaabee.Dapper.Extensions.Enums;

namespace Zaabee.Dapper.Extensions.Adapters
{
    internal class DefaultSqlAdapter : ISqlAdapter
    {
        private readonly ConcurrentDictionary<Type, string> _insertSqlCache = new();

        private readonly ConcurrentDictionary<Type, Dictionary<CriteriaType, string>> _deleteSqlCache = new();

        private readonly ConcurrentDictionary<Type, string> _updateSqlDict = new();

        private readonly ConcurrentDictionary<Type, Dictionary<CriteriaType, string>> _selectSqlCache = new();

        public virtual string GetInsertSql(Type type)
        {
            return _insertSqlCache.GetOrAdd(type, _ =>
            {
                lock (type)
                {
                    var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);

                    var columnNames = new List<string> {typeMapInfo.IdColumnName};
                    columnNames.AddRange(typeMapInfo.PropertyColumnDict.Select(pair => pair.Key));

                    var propertyNames = new List<string> {typeMapInfo.IdPropertyInfo.Name};
                    propertyNames.AddRange(typeMapInfo.PropertyColumnDict.Select(pair => pair.Value.Name));

                    var intoString = string.Join(",", columnNames);
                    var valueString = string.Join(",", propertyNames.Select(propertyName => $"@{propertyName}"));
                    return $"INSERT INTO {typeMapInfo.TableName} ({intoString}) VALUES ({valueString})";
                }
            });
        }

        public virtual string GetDeleteSql(Type type, CriteriaType conditionType)
        {
            var sqls = _deleteSqlCache.GetOrAdd(type, _ =>
            {
                lock (type)
                {
                    var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
                    var fromString = $"DELETE FROM {typeMapInfo.TableName}";
                    return new Dictionary<CriteriaType, string>
                    {
                        {
                            CriteriaType.None,
                            $"{fromString}"
                        },
                        {
                            CriteriaType.SingleId,
                            $"{fromString} {CriteriaTypeStringParse(typeMapInfo, CriteriaType.SingleId)}"
                        },
                        {
                            CriteriaType.MultiId,
                            $"{fromString} {CriteriaTypeStringParse(typeMapInfo, CriteriaType.MultiId)}"
                        }
                    };
                }
            });

            return sqls[conditionType];
        }

        public virtual string GetUpdateSql(Type type)
        {
            return _updateSqlDict.GetOrAdd(type, _ =>
            {
                lock (type)
                {
                    var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
                    var setSql = string.Join(",",
                        typeMapInfo.PropertyColumnDict.Select(pair => $"{pair.Key} = @{pair.Value.Name}"));
                    return
                        $"UPDATE {typeMapInfo.TableName} SET {setSql} {CriteriaTypeStringParse(typeMapInfo, CriteriaType.SingleId)}";
                }
            });
        }

        public virtual string GetSelectSql(Type type, CriteriaType criteriaType)
        {
            var typeSql = _selectSqlCache.GetOrAdd(type, _ =>
            {
                lock (type)
                {
                    var typeMapInfo = TypeMapInfoHelper.GetTypeMapInfo(type);
                    var selectString = SelectStringParse(typeMapInfo);
                    return new Dictionary<CriteriaType, string>
                    {
                        {
                            CriteriaType.None,
                            $"{selectString} "
                        },
                        {
                            CriteriaType.SingleId,
                            $"{selectString} {CriteriaTypeStringParse(typeMapInfo, CriteriaType.SingleId)}"
                        },
                        {
                            CriteriaType.MultiId,
                            $"{selectString} {CriteriaTypeStringParse(typeMapInfo, CriteriaType.MultiId)}"
                        }
                    };
                }
            });

            return typeSql[criteriaType];
        }

        protected virtual string FormatColumnName(string columnName) => $"'{columnName}'";

        protected virtual string SelectStringParse(TypeMapInfo typeMapInfo)
        {
            var selectString =
                $"SELECT {typeMapInfo.TableName}.{typeMapInfo.IdColumnName} AS {FormatColumnName(typeMapInfo.IdPropertyInfo.Name)}, {string.Join(",", typeMapInfo.PropertyColumnDict.Select(pair => $"{typeMapInfo.TableName}.{pair.Key} AS {FormatColumnName(pair.Value.Name)} "))}";
            var fromString = $"FROM {typeMapInfo.TableName} ";
            return $"{selectString}{fromString}";
        }

        protected virtual string CriteriaTypeStringParse(TypeMapInfo typeMapInfo, CriteriaType criteriaType)
        {
            return criteriaType switch
            {
                CriteriaType.None => string.Empty,
                CriteriaType.SingleId => $"WHERE {typeMapInfo.TableName}.{typeMapInfo.IdColumnName} = @Id",
                CriteriaType.MultiId => $"WHERE {typeMapInfo.TableName}.{typeMapInfo.IdColumnName} IN @Ids",
                _ => throw new ArgumentOutOfRangeException(nameof(criteriaType), criteriaType, null)
            };
        }
    }
}