using MySql.Data.MySqlClient;
using Xunit;

namespace Zaabee.Dapper.Extensions.TestProject
{
    public class MySqlTest
    {
        private readonly UnitTest _unitTest;

        public MySqlTest()
        {
            _unitTest = new UnitTest(() => new MySqlConnection(
                "Database=TestDB;Data Source=192.168.78.152;User Id=root;Password=123qweasd,./;CharSet=utf8;port=3306"));
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
        public void RemoveById()
        {
            _unitTest.RemoveById();
        }

        [Fact]
        public void RemoveByEntity()
        {
            _unitTest.RemoveByEntity();
        }

        [Fact]
        public void RemoveAllByIds()
        {
            _unitTest.RemoveAllByIds();
        }

        [Fact]
        public void RemoveAllByEntities()
        {
            _unitTest.RemoveAllByEntities();
        }

        [Fact]
        public void RemoveAll()
        {
            _unitTest.RemoveAll();
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
        public void All()
        {
            _unitTest.All();
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
        public void RemoveByIdAsync()
        {
            _unitTest.RemoveByIdAsync();
        }

        [Fact]
        public void RemoveByEntityAsync()
        {
            _unitTest.RemoveByEntityAsync();
        }

        [Fact]
        public void RemoveAllByIdsAsync()
        {
            _unitTest.RemoveAllByIdsAsync();
        }

        [Fact]
        public void RemoveAllByEntitiesAsync()
        {
            _unitTest.RemoveAllByEntitiesAsync();
        }

        [Fact]
        public void RemoveAllAsync()
        {
            _unitTest.RemoveAllAsync();
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
        public void FirstAsync()
        {
            _unitTest.FirstAsync();
        }

        [Fact]
        public void SingleAsync()
        {
            _unitTest.SingleAsync();
        }

        [Fact]
        public void FirstOrDefaultAsync()
        {
            _unitTest.FirstOrDefaultAsync();
        }

        [Fact]
        public void SingleOrDefaultAsync()
        {
            _unitTest.SingleOrDefaultAsync();
        }

        [Fact]
        public void QueryAsync()
        {
            _unitTest.QueryAsync();
        }

        [Fact]
        public void AllAsync()
        {
            _unitTest.AllAsync();
        }

        #endregion
    }
}