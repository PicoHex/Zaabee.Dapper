using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MySql.Data.MySqlClient;
using Npgsql;
using Xunit;
using Zaabee.Json;
using Zaabee.SequentialGuid;

namespace Zaabee.Dapper.Extensions.TestProject
{
    public class UnitTest1
    {
        #region sync

        [Fact]
        public void Add()
        {
            var myDomainObject = CreateDomainObject();
            int result;
            using (var conn = GetConn())
            {
                result = conn.Add(myDomainObject);
            }

            Assert.Equal(1, result);
        }

        [Fact]
        public void AddRange()
        {
            const int quantity = 10;
            var myDomainObjects = CreateDomainObjects(quantity);
            int result;
            using (var conn = GetConn())
            {
                result = conn.AddRange(myDomainObjects);
            }

            Assert.Equal(quantity, result);
        }

        [Fact]
        public void RemoveById()
        {
            int result;
            using (var conn = GetConn())
            {
                var entity = CreateDomainObject();
                conn.Add(entity);
                result = conn.Remove<MyDomainObject>(entity.Id);
            }

            Assert.Equal(1, result);
        }

        [Fact]
        public void RemoveByEntity()
        {
            int result;
            using (var conn = GetConn())
            {
                var entity = CreateDomainObject();
                conn.Add(entity);
                result = conn.Remove(entity);
            }

            Assert.Equal(1, result);
        }

        [Fact]
        public void RemoveAllByIds()
        {
            int result;
            List<MyDomainObject> entities;
            using (var conn = GetConn())
            {
                entities = CreateDomainObjects(10);
                conn.AddRange(entities);
                result = conn.RemoveAll<MyDomainObject>(entities.Select(entity => entity.Id).ToList());
            }

            Assert.Equal(entities.Count, result);
        }

        [Fact]
        public void RemoveAllByEntities()
        {
            int result;
            List<MyDomainObject> entities;
            using (var conn = GetConn())
            {
                entities = CreateDomainObjects(10);
                conn.AddRange(entities);
                result = conn.RemoveAll(entities);
            }

            Assert.Equal(entities.Count, result);
        }

        [Fact]
        public void RemoveAll()
        {
            int quantity, result;
            using (var conn = GetConn())
            {
                quantity = conn.Query<MyDomainObject>().Count();
                result = conn.RemoveAll<MyDomainObject>();
            }

            Assert.Equal(quantity, result);
        }

        [Fact]
        public void Update()
        {
            using (var conn = GetConn())
            {
                var entity = CreateDomainObject();
                conn.Add(entity);
                entity.Name = "hahahahaha";
                conn.Update(entity);
                var result = conn.QueryFirstOrDefault<MyDomainObject>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        [Fact]
        public void UpdateAll()
        {
            using (var conn = GetConn())
            {
                var entities = CreateDomainObjects(10);
                conn.AddRange(entities);
                entities.ForEach(entity => entity.Name = "hahahahaha");
                conn.UpdateAll(entities);
                var results = conn.Query<MyDomainObject>(entities.Select(entity => entity.Id).ToList()).ToList();
                Assert.Equal(entities.OrderBy(e => e.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"),
                    results.OrderBy(r => r.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"));
            }
        }

        [Fact]
        public void QueryFirst()
        {
            using (var conn = GetConn())
            {
                var entity = CreateDomainObject();
                conn.Add(entity);
                var result = conn.QueryFirst<MyDomainObject>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        [Fact]
        public void QuerySingle()
        {
            using (var conn = GetConn())
            {
                var entity = CreateDomainObject();
                conn.Add(entity);
                var result = conn.QuerySingle<MyDomainObject>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        [Fact]
        public void QueryFirstOrDefault()
        {
            using (var conn = GetConn())
            {
                var entity = CreateDomainObject();
                conn.Add(entity);
                var result = conn.QueryFirstOrDefault<MyDomainObject>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        [Fact]
        public void QuerySingleOrDefault()
        {
            using (var conn = GetConn())
            {
                var entity = CreateDomainObject();
                conn.Add(entity);
                var result = conn.QuerySingleOrDefault<MyDomainObject>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        [Fact]
        public void Query()
        {
            using (var conn = GetConn())
            {
                var entities = CreateDomainObjects(10);
                conn.AddRange(entities);
                var results = conn.Query<MyDomainObject>(entities.Select(e => e.Id).ToList());
                Assert.Equal(entities.OrderBy(e => e.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"),
                    results.OrderBy(r => r.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"));
            }
        }

        [Fact]
        public void QueryAll()
        {
            using (var conn = GetConn())
            {
                var entities = CreateDomainObjects(10);
                conn.AddRange(entities);
                var results = conn.Query<MyDomainObject>();
                Assert.True(results.Count() >= entities.Count);
            }
        }

        #endregion

        #region async

        [Fact]
        public async void AddAsync()
        {
            var myDomainObject = CreateDomainObject();
            int result;
            using (var conn = GetConn())
            {
                result = await conn.AddAsync(myDomainObject);
            }

            Assert.Equal(1, result);
        }

        [Fact]
        public async void AddRangeAsync()
        {
            const int quantity = 10;
            var myDomainObjects = CreateDomainObjects(quantity);
            int result;
            using (var conn = GetConn())
            {
                result = await conn.AddRangeAsync(myDomainObjects);
            }

            Assert.Equal(quantity, result);
        }

        [Fact]
        public async void RemoveByIdAsync()
        {
            int result;
            using (var conn = GetConn())
            {
                var entity = CreateDomainObject();
                await conn.AddAsync(entity);
                result = await conn.RemoveAsync<MyDomainObject>(entity.Id);
            }

            Assert.Equal(1, result);
        }

        [Fact]
        public async void RemoveByEntityAsync()
        {
            int result;
            using (var conn = GetConn())
            {
                var entity = CreateDomainObject();
                await conn.AddAsync(entity);
                result = await conn.RemoveAsync(entity);
            }

            Assert.Equal(1, result);
        }

        [Fact]
        public async void RemoveAllByIdsAsync()
        {
            int result;
            List<MyDomainObject> entities;
            using (var conn = GetConn())
            {
                entities = CreateDomainObjects(10);
                await conn.AddRangeAsync(entities);
                result = await conn.RemoveAllAsync<MyDomainObject>(entities.Select(entity => entity.Id).ToList());
            }

            Assert.Equal(entities.Count, result);
        }

        [Fact]
        public async void RemoveAllByEntitiesAsync()
        {
            int result;
            List<MyDomainObject> entities;
            using (var conn = GetConn())
            {
                entities = CreateDomainObjects(10);
                await conn.AddRangeAsync(entities);
                result = await conn.RemoveAllAsync(entities);
            }

            Assert.Equal(entities.Count, result);
        }

        [Fact]
        public async void RemoveAllAsync()
        {
            int quantity, result;
            using (var conn = GetConn())
            {
                quantity = (await conn.QueryAsync<MyDomainObject>()).Count();
                result = await conn.RemoveAllAsync<MyDomainObject>();
            }

            Assert.Equal(quantity, result);
        }

        [Fact]
        public async void UpdateAsync()
        {
            using (var conn = GetConn())
            {
                var entity = CreateDomainObject();
                await conn.AddAsync(entity);
                entity.Name = "hahahahaha";
                await conn.UpdateAsync(entity);
                var result = await conn.QueryFirstOrDefaultAsync<MyDomainObject>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        [Fact]
        public async void UpdateAllAsync()
        {
            using (var conn = GetConn())
            {
                var entities = CreateDomainObjects(10);
                await conn.AddRangeAsync(entities);
                entities.ForEach(entity => entity.Name = "hahahahaha");
                await conn.UpdateAllAsync(entities);
                var results = await conn.QueryAsync<MyDomainObject>(entities.Select(entity => entity.Id).ToList());
                Assert.Equal(entities.OrderBy(e => e.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"),
                    results.OrderBy(r => r.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"));
            }
        }


        [Fact]
        public async void QueryFirstAsync()
        {
            using (var conn = GetConn())
            {
                var entity = CreateDomainObject();
                await conn.AddAsync(entity);
                var result = await conn.QueryFirstAsync<MyDomainObject>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        [Fact]
        public async void QuerySingleAsync()
        {
            using (var conn = GetConn())
            {
                var entity = CreateDomainObject();
                await conn.AddAsync(entity);
                var result = await conn.QuerySingleAsync<MyDomainObject>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        [Fact]
        public async void QueryFirstOrDefaultAsync()
        {
            using (var conn = GetConn())
            {
                var entity = CreateDomainObject();
                await conn.AddAsync(entity);
                var result = await conn.QueryFirstOrDefaultAsync<MyDomainObject>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        [Fact]
        public async void QuerySingleOrDefaultAsync()
        {
            using (var conn = GetConn())
            {
                var entity = CreateDomainObject();
                await conn.AddAsync(entity);
                var result = await conn.QuerySingleOrDefaultAsync<MyDomainObject>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        [Fact]
        public async void QueryAsync()
        {
            using (var conn = GetConn())
            {
                var entities = CreateDomainObjects(10);
                await conn.AddRangeAsync(entities);
                var results = await conn.QueryAsync<MyDomainObject>(entities.Select(e => e.Id).ToList());
                Assert.Equal(entities.OrderBy(e => e.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"),
                    results.OrderBy(r => r.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"));
            }
        }

        [Fact]
        public async void QueryAllAsync()
        {
            using (var conn = GetConn())
            {
                var entities = CreateDomainObjects(10);
                await conn.AddRangeAsync(entities);
                var results = await conn.QueryAsync<MyDomainObject>();
                Assert.True(results.Count() >= entities.Count);
            }
        }

        #endregion

        private IDbConnection GetConn()
        {
//            return GetPgSqlConn();
//            return GetMySqlConn();
            return GetMsSqlConn();
        }

        private IDbConnection GetPgSqlConn()
        {
            return new NpgsqlConnection(
                "Host=192.168.78.152;Username=postgres;Password=123qweasd,./;Database=postgres");
        }

        private IDbConnection GetMySqlConn()
        {
            return new MySqlConnection(
                "Database=TestDB;Data Source=192.168.78.152;User Id=root;Password=123qweasd,./;CharSet=utf8;port=3306");
        }

        private IDbConnection GetMsSqlConn()
        {
            return new SqlConnection(
                "server=192.168.78.152;database=TestDB;User=sa;password=123qweasd,./;Connect Timeout=30;Pooling=true;Min Pool Size=100;");
        }

        private MyDomainObject CreateDomainObject(SequentialGuidHelper.SequentialGuidType? guidType = null)
        {
            var m = new Random().Next();
            return new MyDomainObject
            {
                Id = guidType == null ? Guid.NewGuid() : SequentialGuidHelper.GenerateComb(guidType.Value),
                Name = m % 3 == 0 ? "apple" : m % 2 == 0 ? "banana" : "pear",
                Gender = m % 2 == 0 ? Gender.Male : Gender.Female,
                Birthday = DateTime.Now,
                CreateTime = DateTime.UtcNow
            };
        }

        private List<MyDomainObject> CreateDomainObjects(int quantity,
            SequentialGuidHelper.SequentialGuidType? guidType = null)
        {
            return Enumerable.Range(0, quantity).Select(p => CreateDomainObject(guidType)).ToList();
        }
    }
}