using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Dapper;

namespace Zaabee.Dapper.Lambda.TestProject.Infrastructure
{
    static class Bootstrap
    {
        public const string ConnectionString = @"Data Source=AppData\Northwind.sdf";

        public static void Initialize()
        {
            InitializeDapper();
        }

        private static void InitializeDapper()
        {
            const string @namespace = "LambdaSqlBuilder.Tests.Entities";

            var entityTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                              where t.IsClass && t.Namespace == @namespace
                              select t;

            foreach (var entityType in entityTypes)
            {
                SqlMapper.SetTypeMap(
                    entityType,
                    new CustomPropertyTypeMap(
                    entityType,
                    (type, columnName) =>
                        type.GetProperties().FirstOrDefault(prop =>
                            prop.GetCustomAttributes(false)
                            .OfType<ColumnAttribute>()
                            .Any(attr => attr.Name == columnName))
                        )
                    );
            }
        }
    }
}
