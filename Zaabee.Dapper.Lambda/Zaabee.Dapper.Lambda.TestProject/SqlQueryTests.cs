using System.Linq;
using Dapper;
using Xunit;
using Zaabee.Dapper.Lambda.TestProject.Entities;
using Zaabee.Dapper.Lambda.TestProject.Infrastructure;

namespace Zaabee.Dapper.Lambda.TestProject
{
    public class SqlQueryTests : TestBase
    {
        /// <summary>
        /// Find the product with name Tofu
        /// </summary>
        [Fact]
        public void FindByFieldValue()
        {
            const string productName = "Tofu";

            var query = new SqlLam<Product>(p => p.ProductName == productName);

            var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.Single(results);
            Assert.Equal(productName, results.First().ProductName);
        }

        /// <summary>
        /// Find products with the reorder level being 5, 15, or 25
        /// </summary>
        [Fact]
        public void FindByListOfValues()
        {
            var reorderLevels = new object[] {5, 15, 25};

            var query = new SqlLam<Product>()
                .WhereIsIn(p => p.ReorderLevel, reorderLevels);

            var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.Equal(30, results.Count);
            Assert.True(results.All(p => reorderLevels.Contains(p.ReorderLevel)));
        }

        /// <summary>
        /// Find products by getting the product Ids first using a subquery
        /// </summary>
        [Fact]
        public void FindBySubQuery()
        {
            var productNames = new object[] { "Konbu", "Tofu", "Pavlova" };

            var subQuery = new SqlLam<Product>()
                .WhereIsIn(p => p.ProductName, productNames)
                .Select(p => p.ProductId);

            var query = new SqlLam<Product>()
                .WhereIsIn(p => p.ProductId, subQuery);

            var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.Equal(3, results.Count);
            Assert.True(results.All(p => productNames.Contains(p.ProductName)));
        }

        /// <summary>
        /// Find products with the reorder level not being 5, 15, or 25
        /// </summary>
        [Fact]
        public void FindByListOfValuesNegated()
        {
            var reorderLevels = new object[] { 5, 15, 25 };

            var query = new SqlLam<Product>()
                .WhereNotIn(p => p.ReorderLevel, reorderLevels);

            var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.Equal(47, results.Count);
            Assert.True(results.All(p => !reorderLevels.Contains(p.ReorderLevel)));
        }

        /// <summary>
        /// Find products not being included in a subquery
        /// </summary>
        [Fact]
        public void FindBySubQueryNegated()
        {
            var productNames = new object[] { "Konbu", "Tofu", "Pavlova" };

            var subQuery = new SqlLam<Product>()
                .WhereIsIn(p => p.ProductName, productNames)
                .Select(p => p.ProductId);

            var query = new SqlLam<Product>()
                .WhereNotIn(p => p.ProductId, subQuery);

            var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.Equal(74, results.Count);
            Assert.True(results.All(p => !productNames.Contains(p.ProductName)));
        }

        /// <summary>
        /// Get product Tofu by its Id and select the value of the Unit Price only
        /// </summary>
        [Fact]
        public void SelectField()
        {
            const int productId = 14;

            var query = new SqlLam<Product>(p => p.ProductId == productId)
                .Select(p => p.UnitPrice);

            var results = Connection.Query<decimal>(query.QueryString, query.QueryParameters).ToList();

            Assert.Single(results);
            Assert.Equal(23.25M, results.First());
        }

        /// <summary>
        /// Get product Tofu by its Id and selects all its properties
        /// </summary>
        [Fact]
        public void SelectAllFields()
        {
            const int productId = 14;

            var query = new SqlLam<Product>(p => p.ProductId == productId)
                .Select(p => p);

            var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).ToList();

            Assert.Single(results);
        }

        /// <summary>
        /// Get categories sorted by name
        /// </summary>
        [Fact]
        public void OrderEntitiesByField()
        {
            var query = new SqlLam<Category>()
                .OrderBy(p => p.CategoryName);

            var results = Connection.Query<Category>(query.QueryString, query.QueryParameters).ToList();

            for (var i = 1; i < results.Count; ++i)
            {
                Assert.True(string.CompareOrdinal(results[i - 1].CategoryName, results[i].CategoryName) <= 0);
            }
        }

        /// <summary>
        /// Get categories sorted by name descending
        /// </summary>
        [Fact]
        public void OrderEntitiesByFieldDescending()
        {
            var query = new SqlLam<Category>()
                .OrderByDescending(p => p.CategoryName);

            var results = Connection.Query<Category>(query.QueryString, query.QueryParameters).ToList();

            for (var i = 1; i < results.Count; ++i)
            {
                Assert.True(string.CompareOrdinal(results[i - 1].CategoryName, results[i].CategoryName) >= 0);
            }
        }

        /// <summary>
        /// Get the number of all products
        /// </summary>
        [Fact]
        public void SelectEntityCount()
        {
            var query = new SqlLam<Product>()
                .SelectCount(p => p.ProductId);

            var resultCount = Connection.Query<int>(query.QueryString, query.QueryParameters).Single();

            Assert.Equal(77, resultCount);
        }

        /// <summary>
        /// Select number of Product IDs for products with the Reorder Level equal to 25
        /// </summary>
        [Fact]
        public void SelectRestrictedEntityCount()
        {
            var query = new SqlLam<Product>()
                .SelectCount(p => p.ProductId)
                .Where(p => p.ReorderLevel == 25);

            var resultCount = Connection.Query<int>(query.QueryString, query.QueryParameters).Single();

            Assert.Equal(12, resultCount);
        }

        /// <summary>
        /// Select number of products for individual Reorder Levels
        /// </summary>
        [Fact]
        public void SelectGroupedCounts()
        {
            var groupSizes = new[] {24, 8, 7, 10, 8, 12, 8};

            var query = new SqlLam<Product>()
                .SelectCount(p => p.ProductId)
                .GroupBy(p => p.ReorderLevel)
                .OrderBy(p => p.ReorderLevel);

            var results = Connection.Query<int>(query.QueryString, query.QueryParameters).ToList();

            Assert.Equal(groupSizes.Length, results.Count);

            for (var i = 0; i < groupSizes.Length; ++i)
            {
                Assert.Equal(groupSizes[i], results[i]);
            }
        }

        /// <summary>
        /// Select all distinct possible values of the Reorder Level
        /// </summary>
        [Fact]
        public void SelectDistinctValues()
        {
            var allValues = new[] {0, 5, 10, 15, 20, 25, 30};

            var query = new SqlLam<Product>()
                .SelectDistinct(p => p.ReorderLevel)
                .OrderBy(p => p.ReorderLevel);

            var results = Connection.Query<short>(query.QueryString, query.QueryParameters).ToList();

            Assert.Equal(allValues.Length, results.Count);
            for (var i = 0; i < allValues.Length; ++i)
            {
                Assert.Equal(allValues[i], results[i]);
            }
        }

        /// <summary>
        /// Select maximum unit price among all the products
        /// </summary>
        [Fact]
        public void SelectMaximumValue()
        {
            const decimal maximumValue = 263.5M;

            var query = new SqlLam<Product>()
                .SelectMax(p => p.UnitPrice);

            var results = Connection.Query<decimal>(query.QueryString, query.QueryParameters).Single();

            Assert.Equal(maximumValue, results);
        }

        /// <summary>
        /// Select minimum unit price among all the products
        /// </summary>
        [Fact]
        public void SelectMinimumValue()
        {
            const decimal minimumValue = 2.5M;

            var query = new SqlLam<Product>()
                .SelectMin(p => p.UnitPrice);

            var results = Connection.Query<decimal>(query.QueryString, query.QueryParameters).Single();

            Assert.Equal(minimumValue, results);
        }

        /// <summary>
        /// Select average unit price among all the products
        /// </summary>
        [Fact]
        public void SelectAverageValue()
        {
            const decimal averageValue = 28.8663M;

            var query = new SqlLam<Product>()
                .SelectAverage(p => p.UnitPrice);

            var results = Connection.Query<decimal>(query.QueryString, query.QueryParameters).Single();

            Assert.Equal(averageValue, results);
        }

        /// <summary>
        /// Select sum of all unit prices among all the products
        /// </summary>
        [Fact]
        public void SelectSum()
        {
            const decimal sum = 2222.71M;

            var query = new SqlLam<Product>()
                .SelectSum(p => p.UnitPrice);

            var results = Connection.Query<decimal>(query.QueryString, query.QueryParameters).Single();

            Assert.Equal(sum, results);
        }

        /// <summary>
        /// Select the product "Tofu" by listing its individual properties using the 'new' construct
        /// </summary>
        [Fact]
        public void SelectWithNew()
        {
            const string productName = "Tofu";

            var query = new SqlLam<Product>()
                .Where(p => p.ProductName == productName)
                .Select(p => new {
                                    p.ProductId,
                                    p.ProductName,
                                    p.CategoryId,
                                    p.ReorderLevel,
                                    p.UnitPrice
                                 });

            var results = Connection.Query<Product>(query.QueryString, query.QueryParameters).Single();

            Assert.NotNull(results.ProductName);
            Assert.Null(results.EnglishName);
        }

        /// <summary>
        /// Load products with reorder level 0 page by page
        /// </summary>
        [Fact]
        public void PaginateOverResults()
        {
            const int reorderLevel = 0;
            const int pageSize = 5;
            const int numberOfPages = 5;
            const int lastPageSize = 4;

            var query = new SqlLam<Product>(p => p.ReorderLevel == reorderLevel)
                .OrderBy(p => p.ProductName);

            for(var page = 1; page < numberOfPages; ++page)
            {
                var results = Connection.Query<Product>(query.QueryStringPage(pageSize, page), query.QueryParameters).ToList();
                Assert.Equal(pageSize, results.Count);
                Assert.True(results.All(p => p.ReorderLevel == reorderLevel));
            }

            var lastResults = Connection.Query<Product>(query.QueryStringPage(pageSize, numberOfPages), query.QueryParameters).ToList();
            Assert.Equal(lastPageSize, lastResults.Count);
            Assert.True(lastResults.All(p => p.ReorderLevel == reorderLevel));
        }

        /// <summary>
        /// Load first 10 products sorted by the product name with reorder level 0 
        /// </summary>
        [Fact]
        public void TopTenResults()
        {
            const int reorderLevel = 0;
            const int pageSize = 10;

            var query = new SqlLam<Product>(p => p.ReorderLevel == reorderLevel)
                .OrderBy(p => p.ProductName);

            var results = Connection.Query<Product>(query.QueryStringPage(pageSize), query.QueryParameters).ToList();
            Assert.Equal(pageSize, results.Count);
            Assert.True(results.All(p => p.ReorderLevel == reorderLevel));
        }
    }
}
