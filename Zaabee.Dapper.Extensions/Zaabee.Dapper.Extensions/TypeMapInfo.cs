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

        private Dictionary<string, PropertyInfo> _propertyColumnDict;

        public Dictionary<string, PropertyInfo> PropertyColumnDict
        {
            get => _propertyColumnDict ?? (_propertyColumnDict = new Dictionary<string, PropertyInfo>());
            set => _propertyColumnDict = value;
        }
    }
}