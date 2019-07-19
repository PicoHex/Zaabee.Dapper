using System;
using System.Collections.Generic;
using System.Reflection;

namespace Zaabee.Dapper.Extensions
{
    public class TypeMapInfo
    {
        public Type Type { get; set; }
        public string TableName { get; set; }
        public PropertyInfo IdPropertyInfo { get; set; }
        public string IdColumnName { get; set; }
        public Dictionary<string, PropertyInfo> PropertyColumnDict { get; } = new Dictionary<string, PropertyInfo>();
        public Dictionary<string, TypeMapInfo> PropertyTableDict { get; } = new Dictionary<string, TypeMapInfo>();
    }
}