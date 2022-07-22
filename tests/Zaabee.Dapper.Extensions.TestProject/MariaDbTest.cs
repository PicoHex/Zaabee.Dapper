using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Xunit;

namespace Zaabee.Dapper.Extensions.TestProject
{
    public class MariaDbTest
    {
        private readonly UnitTest _unitTest;

        public MariaDbTest()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetConnectionString("MariaDB");
            _unitTest = new UnitTest(() => new MySqlConnection(config.GetConnectionString("MariaDB")));
        }

        #region sync

        [Fact]
        public void Add()
        {
            _unitTest.Add();
        }

        [Fact]
        public void AddRange()
        {
            _unitTest.AddRange();
        }

        [Fact]
        public void DeleteById()
        {
            _unitTest.DeleteById();
        }

        [Fact]
        public void DeleteByEntity()
        {
            _unitTest.DeleteByEntity();
        }

        [Fact]
        public void DeleteAllByIds()
        {
            _unitTest.DeleteAllByIds();
        }

        [Fact]
        public void DeleteAllByEntities()
        {
            _unitTest.DeleteAllByEntities();
        }

        [Fact]
        public void DeleteAll()
        {
            _unitTest.DeleteAll();
        }

        [Fact]
        public void Update()
        {
            _unitTest.Update();
        }

        [Fact]
        public void UpdateAll()
        {
            _unitTest.UpdateAll();
        }

        [Fact]
        public void FirstOrDefault()
        {
            _unitTest.FirstOrDefault();
        }

        [Fact]
        public void Query()
        {
            _unitTest.Query();
        }

        [Fact]
        public void GetAll()
        {
            _unitTest.GetAll();
        }

        #endregion

        #region async

        [Fact]
        public void AddAsync()
        {
            _unitTest.AddAsync();
        }

        [Fact]
        public void AddRangeAsync()
        {
            _unitTest.AddRangeAsync();
        }

        [Fact]
        public void DeleteByIdAsync()
        {
            _unitTest.DeleteByIdAsync();
        }

        [Fact]
        public void DeleteByEntityAsync()
        {
            _unitTest.DeleteByEntityAsync();
        }

        [Fact]
        public void DeleteAllByIdsAsync()
        {
            _unitTest.DeleteAllByIdsAsync();
        }

        [Fact]
        public void DeleteAllByEntitiesAsync()
        {
            _unitTest.DeleteAllByEntitiesAsync();
        }

        [Fact]
        public void DeleteAllAsync()
        {
            _unitTest.DeleteAllAsync();
        }

        [Fact]
        public void UpdateAsync()
        {
            _unitTest.UpdateAsync();
        }

        [Fact]
        public void UpdateAllAsync()
        {
            _unitTest.UpdateAllAsync();
        }

        [Fact]
        public void FirstOrDefaultAsync()
        {
            _unitTest.FirstOrDefaultAsync();
        }

        [Fact]
        public void AllAsync()
        {
            _unitTest.AllAsync();
        }

        #endregion
    }
}