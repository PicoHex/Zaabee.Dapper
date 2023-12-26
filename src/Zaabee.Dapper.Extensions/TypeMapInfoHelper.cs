namespace Zaabee.Dapper.Extensions;

internal static class TypeMapInfoHelper
{
    private static readonly ConcurrentDictionary<Type, TypeMapInfo> TypePropertyCache = new();

    public static object GetIdValue<T>(T entity) =>
        GetTypeMapInfo(typeof(T)).IdPropertyInfo?.GetValue(entity);

    internal static TypeMapInfo GetTypeMapInfo(Type type)
    {
        return TypePropertyCache.GetOrAdd(
            type,
            _ =>
            {
                lock (type)
                {
                    var typeMapInfo = new TypeMapInfo
                    {
                        TableName =
                            Attribute
                                .GetCustomAttributes(type)
                                .OfType<TableAttribute>()
                                .FirstOrDefault()
                                ?.Name ?? type.Name
                    };

                    var typeProperties = type.GetProperties()
                        .Where(
                            p =>
                                !Attribute.GetCustomAttributes(p).OfType<NotMappedAttribute>().Any()
                        )
                        .ToList();

                    typeMapInfo.IdPropertyInfo = typeProperties.FirstOrDefault(
                        property =>
                            Attribute.GetCustomAttributes(property).OfType<KeyAttribute>().Any()
                            || property.Name is "Id" or "ID" or "id" or "_id"
                            || property.Name == $"{typeMapInfo.TableName}Id"
                    );

                    if (typeMapInfo.IdPropertyInfo is null)
                        throw new ArgumentException(
                            $"Can not find the id property in {nameof(type)}."
                        );

                    typeMapInfo.IdColumnName =
                        Attribute
                            .GetCustomAttributes(typeMapInfo.IdPropertyInfo)
                            .OfType<ColumnAttribute>()
                            .FirstOrDefault()
                            ?.Name ?? typeMapInfo.IdPropertyInfo.Name;

                    typeMapInfo.PropertyColumnDict = typeProperties
                        .Where(property => property != typeMapInfo.IdPropertyInfo)
                        .ToDictionary(
                            k =>
                                Attribute
                                    .GetCustomAttributes(k)
                                    .OfType<ColumnAttribute>()
                                    .FirstOrDefault()
                                    ?.Name ?? k.Name,
                            v => v
                        );

                    return typeMapInfo;
                }
            }
        );
    }
}
