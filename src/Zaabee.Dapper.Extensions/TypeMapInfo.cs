namespace Zaabee.Dapper.Extensions;

internal class TypeMapInfo
{
    public string TableName { get; set; } = string.Empty;
    public PropertyInfo? IdPropertyInfo { get; set; }
    public string IdColumnName { get; set; } = string.Empty;

    public Dictionary<string, PropertyInfo> PropertyColumnDict { get; set; } = new();
}