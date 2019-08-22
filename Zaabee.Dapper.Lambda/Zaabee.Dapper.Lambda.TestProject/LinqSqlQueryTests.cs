using System;
using System.Linq;
using Dapper;
using Xunit;
using Zaabee.Dapper.Lambda.TestProject.Entities;
using Zaabee.Dapper.Lambda.TestProject.Infrastructure;

namespace Zaabee.Dapper.Lambda.TestProject
{
    public class LinqSqlQueryTests : TestBase
    {
        /// <summary>
        /// Find the product with name Tofu
        /// </summary>
        [Fact]
        public void FindByFieldValue()
        {
            const string productName = "Tofu";

            var query = from product in new SqlLam<Product>()
                        where product.ProductName == productName
                        select product;

            var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.Single(results);
            Assert.Equal(productName, results.First().ProductName);
        }

        /// <summary>
        /// Get product Tofu by its Id and select the value of the Unit Price only
        /// </summary>
        [Fact]
        public void SelectField()
        {
            const int productId = 14;

            var query = from product in new SqlLam<Product>()
                        where product.ProductId == productId
                        select product.UnitPrice;

            var results = Connection.Query<decimal>(query.QueryString, query.QueryParameters).ToList();

            Assert.Single(results);
            Assert.Equal(23.25M, results.First());
        }

        /// <summary>
        /// Find all products for the category Beverages and the Reorder Level 25
        /// </summary>
        [Fact]
        public void FindByJoinedEntityValue()
        {
            const int reorderLevel = 25;
            const string categoryName = "Beverages";
            const int categoryId = 1;

            var query = from product in new SqlLam<Product>()
                        join category in new SqlLam<Category>()
                        on product.CategoryId equals category.CategoryId
                        where product.ReorderLevel == reorderLevel && category.CategoryName == categoryName
                        select product;

            var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.Equal(3, results.Count);
            Assert.True(results.All(p => p.CategoryId == categoryId));
        }

        /// <summary>
        /// Get categories sorted by name
        /// </summary>
        [Fact]
        public void OrderEntitiesByField()
        {
            var query = from category in new SqlLam<Category>()
                        orderby category.CategoryName
                        select category;

            var results = Connection.Query<Category>(query.QueryString, query.QueryParameters).ToList();

            for (int i = 1; i < results.Count; ++i)
            {
                Assert.True(String.CompareOrdinal(results[i - 1].CategoryName, results[i].CategoryName) <= 0);
            }
        }

        /// <summary>
        /// Get categories sorted by name descending
        /// </summary>
        [Fact]
        public void OrderEntitiesByFieldDescending()
        {
            var query = from category in new SqlLam<Category>()
                        orderby category.CategoryName descending
                        select category;

            var results = Connection.Query<Category>(query.QueryString, query.QueryParameters).ToList();

            for (int i = 1; i < results.Count; ++i)
            {
                Assert.True(String.CompareOrdinal(results[i - 1].CategoryName, results[i].CategoryName) >= 0);
            }
        }

        /// <summary>
        /// Select number of products for individual Reorder Levels
        /// </summary>
        [Fact]
        public void SelectGroupedCounts()
        {
            var groupSizes = new[] { 24, 8, 7, 10, 8, 12, 8 };

            var query = from product in new SqlLam<Product>()
                        group product by product.ReorderLevel;

            query.SelectCount(p => p.ProductId);                                               

            var results = Connection.Query<int>(query.QueryString, query.QueryParameters).ToList();

            Assert.Equal(groupSizes.Length, results.Count);

            for (int i = 0; i < groupSizes.Length; ++i)
            {
                Assert.Equal(groupSizes[i], results[i]);
            }
        }
    }
}
