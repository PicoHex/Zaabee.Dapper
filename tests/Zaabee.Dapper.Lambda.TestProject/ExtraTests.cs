using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Xunit;
using Zaabee.Dapper.Lambda.TestProject.Entities;
using Zaabee.Dapper.Lambda.TestProject.Infrastructure;
using Zaabee.Dapper.Lambda.ValueObjects;

namespace Zaabee.Dapper.Lambda.TestProject
{
    public class ExtraTests : TestBase
    {
        /// <summary>
        /// Access the underlying SqlBuilder to directly specify the selection and the having condition
        /// </summary>
        [Fact]
        public void DirectSqlBuilderAccess()
        {
            var query = from product in new SqlLam<Product>()
                        where product.ReorderLevel > 10
                        group product by product.ReorderLevel;

            query.SqlBuilder.SelectionList.Add("MAX([Unit Price]) AS MaxPrice");
            query.SqlBuilder.HavingConditions.Add("MAX([Unit Price]) < @PriceParam");
            query.SqlBuilder.Parameters.Add("PriceParam", 25M);

            var result = Connection.Query<decimal>(query.QueryString, query.QueryParameters).ToList();

            Assert.Single(result);
            Assert.Equal(24, result.First());
        }

        /// <summary>
        /// Load all product names without using the Dapper only with the standard Sql Command and the Data Reader
        /// </summary>
        [Fact]
        public void DapperLessExecution()
        {
            var query = from product in new SqlLam<Product>()
                        orderby product.ProductName
                        select product.ProductName;

            var selectCommand = new SqlCommand(query.QueryString, Connection);
            foreach (var param in query.QueryParameters)
                selectCommand.Parameters.AddWithValue(param.Key, param.Value);

            var result = selectCommand.ExecuteReader();

            int count = 0;
            while(result.Read())
            {
                ++count;
                Assert.NotNull(result.GetString(0));
            }
            Assert.Equal(77, count);
        }

        /// <summary>
        /// Change the adapter to Sql Server 2008 and verify whether the pagination SQL string contains the specific keyword ROW_NUMBER
        /// </summary>
        [Fact]
        public void ChangeSqlAdapter()
        {
            SqlLamBase.SetAdapter(SqlAdapter.SqlServer2008);

            var query = from product in new SqlLam<Product>()
                        orderby product.ProductName
                        select product;

            var queryString = query.QueryStringPage(10, 1);

            SqlLamBase.SetAdapter(SqlAdapter.SqlServer2012);

            Assert.Contains("ROW_NUMBER", queryString);
        }
    }
}
