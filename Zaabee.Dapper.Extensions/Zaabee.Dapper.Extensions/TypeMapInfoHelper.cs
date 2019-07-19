using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Zaabee.Dapper.Extensions
{
    public static class TypeMapInfoHelper
    {
        private static readonly ConcurrentDictionary<Type, TypeMapInfo> TypePropertyCache =
            new ConcurrentDictionary<Type, TypeMapInfo>();

        public static object GetIdValue<T>(T entity)
        {
            var typeMapInfo = GetTypeMapInfo(typeof(T));
            return typeMapInfo.IdPropertyInfo.GetValue(entity);
        }

        public static TypeMapInfo GetTypeMapInfo(Type type)
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

                typeMapInfo.IdPropertyInfo =
                    typeProperties.FirstOrDefault(property =>
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
    }
}