using System.Collections.Generic;
using System.Reflection;

namespace Zaabee.Dapper.Extensions
{
    internal class TypeMapInfo
    {
        public TypeInfo TypeInfo { get; set; }
        public string TableName { get; set; }
        public PropertyInfo IdPropertyInfo { get; set; }
        public string IdColumnName { get; set; }
        public PropertyInfo ForeignKeyPropertyInfo { get; set; }
        public string ForeignKeyColumnName { get; set; }
        public Dictionary<string, PropertyInfo> PropertyColumnDict { get; } = new();
        public Dictionary<PropertyInfo, TypeMapInfo> PropertyTableDict { get; } = new();
    }
}