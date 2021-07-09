using System.Collections.Generic;
using System.Reflection;

namespace Zaabee.Dapper.Extensions
{
    internal class TypeMapInfo
    {
        public string TableName { get; set; }
        public string TableAlias { get; set; }
        public PropertyInfo IdPropertyInfo { get; set; }
        public string IdColumnName { get; set; }
        public Dictionary<string, PropertyInfo> PropertyColumnDict { get; set; }
    }
}