using System.Data.SqlClient;
using Xunit;

namespace Zaabee.Dapper.Extensions.TestProject
{
    public class SqlServerTest
    {
        private readonly UnitTest _unitTest;

        public SqlServerTest()
        {
            _unitTest = new UnitTest(() => new SqlConnection(
                "server=192.168.78.152;database=TestDB;User=sa;password=123qweasd,./;Connect Timeout=30;Pooling=true;Min Pool Size=100;"));
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
        public void First()
        {
            _unitTest.First();
        }

        [Fact]
        public void Single()
        {
            _unitTest.Single();
        }

        [Fact]
        public void FirstOrDefault()
        {
            _unitTest.FirstOrDefault();
        }

        [Fact]
        public void SingleOrDefault()
        {
            _unitTest.SingleOrDefault();
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
        public async void AddAsync()
        {
            _unitTest.AddAsync();
        }

        [Fact]
        public async void AddRangeAsync()
        {
            _unitTest.AddRangeAsync();
        }

        [Fact]
        public async void RemoveByIdAsync()
        {
            _unitTest.RemoveByIdAsync();
        }

        [Fact]
        public async void RemoveByEntityAsync()
        {
            _unitTest.RemoveByEntityAsync();
        }

        [Fact]
        public async void RemoveAllByIdsAsync()
        {
            _unitTest.RemoveAllByIdsAsync();
        }

        [Fact]
        public async void RemoveAllByEntitiesAsync()
        {
            _unitTest.RemoveAllByEntitiesAsync();
        }

        [Fact]
        public async void RemoveAllAsync()
        {
            _unitTest.RemoveAllAsync();
        }

        [Fact]
        public async void UpdateAsync()
        {
            _unitTest.UpdateAsync();
        }

        [Fact]
        public async void UpdateAllAsync()
        {
            _unitTest.UpdateAllAsync();
        }


        [Fact]
        public async void FirstAsync()
        {
            _unitTest.FirstAsync();
        }

        [Fact]
        public async void SingleAsync()
        {
            _unitTest.SingleAsync();
        }

        [Fact]
        public async void FirstOrDefaultAsync()
        {
            _unitTest.FirstOrDefaultAsync();
        }

        [Fact]
        public async void SingleOrDefaultAsync()
        {
            _unitTest.SingleOrDefaultAsync();
        }

        [Fact]
        public async void QueryAsync()
        {
            _unitTest.QueryAsync();
        }

        [Fact]
        public async void AllAsync()
        {
            _unitTest.AllAsync();
        }

        #endregion
    }
}