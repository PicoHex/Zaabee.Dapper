﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;

namespace Zaabee.Dapper.Extensions
{
    public static class DapperExtensions
    {
        private static readonly ConcurrentDictionary<Type, TypeMapInfo> TypePropertyCache =
            new ConcurrentDictionary<Type, TypeMapInfo>();

        private static readonly ConcurrentDictionary<Type, string> InsertSqlCache =
            new ConcurrentDictionary<Type, string>();

        private static readonly ConcurrentDictionary<Type, Dictionary<CriteriaType, string>> DeleteSqlCache =
            new ConcurrentDictionary<Type, Dictionary<CriteriaType, string>>();

        private static readonly ConcurrentDictionary<Type, string> UpdateSqlDict =
            new ConcurrentDictionary<Type, string>();

        private static readonly ConcurrentDictionary<Type, Dictionary<SelectType, Dictionary<CriteriaType, string>>>
            SelectSqlCache =
                new ConcurrentDictionary<Type, Dictionary<SelectType, Dictionary<CriteriaType, string>>>();

        #region sync

        public static int Add<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Execute(GetInsertSql(typeof(T)), persistentObject, transaction, commandTimeout,
                commandType);
        }

        public static int AddRange<T>(this IDbConnection connection, List<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Execute(GetInsertSql(typeof(T)), persistentObjects, transaction, commandTimeout,
                commandType);
        }

        public static int Remove<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Remove<T>(connection, GetIdValue(persistentObject), transaction, commandTimeout, commandType);
        }

        public static int Remove<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Execute(
                GetDeleteSql(typeof(T), CriteriaType.Single),
                new {Id = id},
                transaction, commandTimeout, commandType);
        }

        public static int RemoveAll<T>(this IDbConnection connection, List<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var ids = persistentObjects.Select(GetIdValue).Cast<Guid>().ToList();
            return RemoveAll<T>(connection, ids, transaction, commandTimeout, commandType);
        }

        public static int RemoveAll<T>(this IDbConnection connection, object ids,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Execute(GetDeleteSql(typeof(T), CriteriaType.Multi), new {Ids = (IEnumerable) ids},
                transaction,
                commandTimeout,
                commandType);
        }

        public static int RemoveAll<T>(this IDbConnection connection,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Execute(GetDeleteSql(typeof(T), CriteriaType.All), null, transaction, commandTimeout,
                commandType);
        }

        public static int Update<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Execute(GetUpdateSql(typeof(T)), persistentObject, transaction, commandTimeout,
                commandType);
        }

        public static int UpdateAll<T>(this IDbConnection connection, List<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.Execute(GetUpdateSql(typeof(T)), persistentObjects, transaction, commandTimeout,
                commandType);
        }

        public static T QuerySingle<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingle<T>(GetSelectSql(typeof(T), SelectType.AllFields, CriteriaType.Single),
                new {Id = id}, transaction,
                commandTimeout, commandType);
        }

        public static T QueryFirst<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirst<T>(GetSelectSql(typeof(T), SelectType.AllFields, CriteriaType.Single),
                new {Id = id}, transaction,
                commandTimeout, commandType);
        }

        public static T QuerySingleOrDefault<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QuerySingleOrDefault<T>(
                GetSelectSql(typeof(T), SelectType.AllFields, CriteriaType.Single), new {Id = id}, transaction,
                commandTimeout, commandType);
        }

        public static T QueryFirstOrDefault<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return connection.QueryFirstOrDefault<T>(GetSelectSql(typeof(T), SelectType.AllFields, CriteriaType.Single),
                new {Id = id}, transaction,
                commandTimeout, commandType);
        }

        public static IEnumerable<T> Query<T>(this IDbConnection connection, object ids,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return connection.Query<T>(GetSelectSql(typeof(T), SelectType.AllFields, CriteriaType.Multi),
                new {Ids = (IEnumerable) ids}, transaction,
                buffered,
                commandTimeout, commandType);
        }

        public static IEnumerable<T> Query<T>(this IDbConnection connection,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return connection.Query<T>(GetSelectSql(typeof(T), SelectType.AllFields, CriteriaType.All), null,
                transaction, buffered,
                commandTimeout,
                commandType);
        }

        #endregion

        #region async

        public static async Task<int> AddAsync<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.ExecuteAsync(GetInsertSql(typeof(T)), persistentObject, transaction, commandTimeout,
                commandType);
        }

        public static async Task<int> AddRangeAsync<T>(this IDbConnection connection, List<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.ExecuteAsync(GetInsertSql(typeof(T)), persistentObjects, transaction,
                commandTimeout, commandType);
        }

        public static async Task<int> RemoveAsync<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var id = GetIdValue(persistentObject);
            return await RemoveAsync<T>(connection, id, transaction, commandTimeout, commandType);
        }

        public static async Task<int> RemoveAsync<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.ExecuteAsync(
                GetDeleteSql(typeof(T), CriteriaType.Single),
                new {Id = id},
                transaction, commandTimeout, commandType);
        }

        public static async Task<int> RemoveAllAsync<T>(this IDbConnection connection, List<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var ids = persistentObjects.Select(GetIdValue).Cast<Guid>().ToList();
            return await RemoveAllAsync<T>(connection, ids, transaction, commandTimeout, commandType);
        }

        public static async Task<int> RemoveAllAsync<T>(this IDbConnection connection, object ids,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.ExecuteAsync(GetDeleteSql(typeof(T), CriteriaType.Multi),
                new {Ids = (IEnumerable) ids}, transaction, commandTimeout, commandType);
        }

        public static async Task<int> RemoveAllAsync<T>(this IDbConnection connection,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.ExecuteAsync(GetDeleteSql(typeof(T), CriteriaType.All), null, transaction,
                commandTimeout,
                commandType);
        }

        public static async Task<int> UpdateAsync<T>(this IDbConnection connection, T persistentObject,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.ExecuteAsync(GetUpdateSql(typeof(T)), persistentObject, transaction, commandTimeout,
                commandType);
        }

        public static async Task<int> UpdateAllAsync<T>(this IDbConnection connection, List<T> persistentObjects,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.ExecuteAsync(GetUpdateSql(typeof(T)),
                persistentObjects, transaction, commandTimeout, commandType);
        }

        public static async Task<T> QuerySingleAsync<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.QuerySingleAsync<T>(
                GetSelectSql(typeof(T), SelectType.AllFields, CriteriaType.Single), new {Id = id}, transaction,
                commandTimeout, commandType);
        }

        public static async Task<T> QueryFirstAsync<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.QueryFirstAsync<T>(
                GetSelectSql(typeof(T), SelectType.AllFields, CriteriaType.Single), new {Id = id}, transaction,
                commandTimeout, commandType);
        }

        public static async Task<T> QuerySingleOrDefaultAsync<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.QuerySingleOrDefaultAsync<T>(
                GetSelectSql(typeof(T), SelectType.AllFields, CriteriaType.Single), new {Id = id},
                transaction, commandTimeout, commandType);
        }

        public static async Task<T> QueryFirstOrDefaultAsync<T>(this IDbConnection connection, object id,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(
                GetSelectSql(typeof(T), SelectType.AllFields, CriteriaType.Single), new {Id = id},
                transaction, commandTimeout, commandType);
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection connection, object ids,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return await connection.QueryAsync<T>(GetSelectSql(typeof(T), SelectType.AllFields, CriteriaType.Multi),
                new {Ids = (IEnumerable) ids},
                transaction,
                commandTimeout, commandType);
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection connection,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return await connection.QueryAsync<T>(GetSelectSql(typeof(T), SelectType.AllFields, CriteriaType.All), null,
                transaction,
                commandTimeout,
                commandType);
        }

        #endregion

        #region sql

        private static string GetInsertSql(Type type)
        {
            return InsertSqlCache.GetOrAdd(type, key =>
            {
                var typeMapInfo = GetTypeMapInfo(type);

                var columnNames = new List<string> {typeMapInfo.IdColumnName};
                columnNames.AddRange(typeMapInfo.PropertyColumnDict.Select(pair => pair.Key));

                var propertyNames = new List<string> {typeMapInfo.IdPropertyInfo.Name};
                propertyNames.AddRange(typeMapInfo.PropertyColumnDict.Select(pair => pair.Value.Name));

                var intoString = string.Join(",", columnNames);
                var valueString = string.Join(",", propertyNames.Select(propertyName => $"@{propertyName}"));
                return $"INSERT INTO {typeMapInfo.TableName} ({intoString}) VALUES ({valueString})";
            });
        }

        private static string GetDeleteSql(Type type, CriteriaType conditionType)
        {
            var typeMapInfo = GetTypeMapInfo(type);
            var sqls = DeleteSqlCache.GetOrAdd(type, typeKey => new Dictionary<CriteriaType, string>
            {
                {CriteriaType.Single, $"DELETE FROM {typeMapInfo.TableName} WHERE {typeMapInfo.IdColumnName} = @id"},
                {CriteriaType.Multi,$"DELETE FROM {typeMapInfo.TableName} WHERE {typeMapInfo.IdColumnName} = Any(@ids)"},
                {CriteriaType.All, $"DELETE FROM {typeMapInfo.TableName}"}
            });

            return sqls[conditionType];
        }

        private static string GetUpdateSql(Type type)
        {
            var typeMapInfo = GetTypeMapInfo(type);
            var setSql = string.Join(",",
                typeMapInfo.PropertyColumnDict.Select(pair => $"{pair.Key} = @{pair.Value.Name}"));
            return UpdateSqlDict.GetOrAdd(type,
                key =>
                    $"UPDATE {typeMapInfo.TableName} SET {setSql} WHERE {typeMapInfo.IdColumnName} = @{typeMapInfo.IdPropertyInfo.Name}");
        }

        private static string GetSelectSql(Type type, SelectType selectType, CriteriaType criteriaType)
        {
            var typeMapInfo = GetTypeMapInfo(type);
            var propertyInfoDict = new Dictionary<string, PropertyInfo>
                {{typeMapInfo.IdColumnName, typeMapInfo.IdPropertyInfo}};
            foreach (var pair in typeMapInfo.PropertyColumnDict)
                propertyInfoDict.Add(pair.Key, pair.Value);

            var typeSql = SelectSqlCache.GetOrAdd(type, key =>
            {
                var selectAllFieldsString = $"SELECT {string.Join(",", propertyInfoDict.Select(pair => $"{pair.Key} AS {pair.Value.Name}"))}";
                var selectCountByIdString = $"SELECT COUNT({typeMapInfo.IdColumnName})";
                const string selectCountBy1String = "SELECT COUNT(1)";
                const string  selectCountByStarString = "SELECT COUNT(*)";
                var fromString = $"FROM {typeMapInfo.TableName}";
                var whereEqualIdString = $"WHERE {typeMapInfo.IdColumnName} = @id";
                var whereAnyIdsString = $"WHERE {typeMapInfo.IdColumnName} = Any(@ids)";
                
                return new Dictionary<SelectType, Dictionary<CriteriaType, string>>
                {
                    {
                        SelectType.AllFields, new Dictionary<CriteriaType, string>
                        {
                            {
                                CriteriaType.Single,
                                $"{selectAllFieldsString} {fromString} {whereEqualIdString}"
                            },
                            {
                                CriteriaType.Multi,
                                $"{selectAllFieldsString} {fromString} {whereAnyIdsString}"
                            },
                            {
                                CriteriaType.All,
                                $"{selectAllFieldsString} {fromString}"
                            }
                        }
                    },
                    {
                        SelectType.CountById, new Dictionary<CriteriaType, string>
                        {
                            {
                                CriteriaType.Single,
                                $"{selectCountByIdString} {fromString} {whereEqualIdString}"
                            },
                            {
                                CriteriaType.Multi,
                                $"{selectCountByIdString} {fromString} {whereAnyIdsString}"
                            },
                            {
                                CriteriaType.All,
                                $"{selectCountByIdString} {fromString}"
                            }
                        }
                    },
                    {
                        SelectType.CountBy1, new Dictionary<CriteriaType, string>
                        {
                            {
                                CriteriaType.Single,
                                $"{selectCountBy1String} {fromString} {whereEqualIdString}"
                            },
                            {
                                CriteriaType.Multi,
                                $"{selectCountBy1String} {fromString} {whereAnyIdsString}"
                            },
                            {
                                CriteriaType.All,
                                $"{selectCountBy1String} {fromString}"
                            }
                        }
                    },
                    {
                        SelectType.CountByStar, new Dictionary<CriteriaType, string>
                        {
                            {
                                CriteriaType.Single,
                                $"{selectCountByStarString} {fromString} {whereEqualIdString}"
                            },
                            {
                                CriteriaType.Multi,
                                $"{selectCountByStarString} {fromString} {whereAnyIdsString}"
                            },
                            {
                                CriteriaType.All,
                                $"{selectCountByStarString} {fromString}"
                            }
                        }
                    },
                };
            });

            return typeSql[selectType][criteriaType];
        }

        #endregion

        private static TypeMapInfo GetTypeMapInfo(Type type)
        {
            return TypePropertyCache.GetOrAdd(type, typeKey =>
            {
                var typeMapInfo = new TypeMapInfo
                {
                    Type = type,
                    TableName = Attribute.GetCustomAttributes(type).OfType<TableAttribute>().FirstOrDefault()?.Name ??
                                type.Name
                };
                var typeProperties = type.GetProperties();

                typeMapInfo.IdPropertyInfo = typeProperties.FirstOrDefault(property =>
                    Attribute.GetCustomAttributes(property).OfType<KeyAttribute>().Any() ||
                    property.Name == "Id" ||
                    property.Name == "ID" ||
                    property.Name == "id" ||
                    property.Name == "_id" ||
                    property.Name == $"{typeMapInfo.TableName}Id");

                if (typeMapInfo.IdPropertyInfo == null)
                    throw new ArgumentException($"Can not find the id property in {nameof(type)}.");

                typeMapInfo.IdColumnName =
                    Attribute.GetCustomAttributes(typeMapInfo.IdPropertyInfo).OfType<ColumnAttribute>().FirstOrDefault()
                        ?.Name ?? typeMapInfo.IdPropertyInfo.Name;

                foreach (var propertyInfo in typeProperties.Where(property => property != typeMapInfo.IdPropertyInfo))
                    typeMapInfo.PropertyColumnDict.Add(
                        Attribute.GetCustomAttributes(propertyInfo).OfType<ColumnAttribute>().FirstOrDefault()?.Name ??
                        propertyInfo.Name, propertyInfo);

                return typeMapInfo;
            });
        }

        private static object GetIdValue<T>(T entity)
        {
            var typeMapInfo = GetTypeMapInfo(typeof(T));
            return typeMapInfo.IdPropertyInfo.GetValue(entity);
        }
    }
}