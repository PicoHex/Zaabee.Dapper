using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;
using Zaabee.NewtonsoftJson;
using Zaabee.SequentialGuid;

namespace Zaabee.Dapper.Extensions.TestProject
{
    public class UnitTest
    {
        private readonly Func<IDbConnection> _connFunc;
        public UnitTest(Func<IDbConnection> connFunc)
        {
            _connFunc = connFunc;
        }
        
        #region sync

        public void Add()
        {
            var myPoco = CreatePoco();
            int result;
            using (var conn = _connFunc())
            {
                result = conn.Add(myPoco);
            }

            Assert.Equal(1, result);
        }

        public void AddRange()
        {
            const int quantity = 10;
            var myPocos = CreatePocos(quantity);
            int result;
            using (var conn = _connFunc())
            {
                result = conn.AddRange(myPocos);
            }

            Assert.Equal(quantity, result);
        }

        public void RemoveById()
        {
            int result;
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                conn.Add(entity);
                result = conn.Remove<MyPoco>(entity.Id);
            }

            Assert.Equal(1, result);
        }

        public void RemoveByEntity()
        {
            int result;
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                conn.Add(entity);
                result = conn.Remove(entity);
            }

            Assert.Equal(1, result);
        }

        public void RemoveAllByIds()
        {
            int result;
            List<MyPoco> entities;
            using (var conn = _connFunc())
            {
                entities = CreatePocos(10);
                conn.AddRange(entities);
                result = conn.RemoveAll<MyPoco>(entities.Select(entity => entity.Id).ToList());
            }

            Assert.Equal(entities.Count, result);
        }

        public void RemoveAllByEntities()
        {
            int result;
            List<MyPoco> entities;
            using (var conn = _connFunc())
            {
                entities = CreatePocos(10);
                conn.AddRange(entities);
                result = conn.RemoveAll(entities);
            }

            Assert.Equal(entities.Count, result);
        }

        public void RemoveAll()
        {
            int quantity, result;
            using (var conn = _connFunc())
            {
                quantity = conn.Query<MyPoco>().Count();
                result = conn.RemoveAll<MyPoco>();
            }

            Assert.Equal(quantity, result);
        }

        public void Update()
        {
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                conn.Add(entity);
                entity.Name = "hahahahaha";
                var modifyQuantity = conn.Update(entity);
                Assert.Equal(1, modifyQuantity);
                var result = conn.FirstOrDefault<MyPoco>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        public void UpdateAll()
        {
            using (var conn = _connFunc())
            {
                var entities = CreatePocos(10);
                conn.AddRange(entities);
                entities.ForEach(entity => entity.Name = "hahahahaha");
                var modifyQuantity = conn.UpdateAll(entities);
                Assert.Equal(modifyQuantity, entities.Count);
                var results = conn.Query<MyPoco>(entities.Select(entity => entity.Id).ToList()).ToList();
                Assert.Equal(entities.OrderBy(e => e.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"),
                    results.OrderBy(r => r.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"));
            }
        }

        public void First()
        {
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                conn.Add(entity);
                var result = conn.First<MyPoco>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        public void Single()
        {
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                conn.Add(entity);
                var result = conn.Single<MyPoco>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        public void FirstOrDefault()
        {
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                conn.Add(entity);
                var result = conn.FirstOrDefault<MyPoco>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        public void SingleOrDefault()
        {
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                conn.Add(entity);
                var result = conn.SingleOrDefault<MyPoco>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        public void Query()
        {
            using (var conn = _connFunc())
            {
                var entities = CreatePocos(10);
                conn.AddRange(entities);
                var results = conn.Query<MyPoco>(entities.Select(e => e.Id).ToList());
                Assert.Equal(entities.OrderBy(e => e.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"),
                    results.OrderBy(r => r.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"));
            }
        }

        public void All()
        {
            using (var conn = _connFunc())
            {
                var entities = CreatePocos(10);
                conn.AddRange(entities);
                var results = conn.Query<MyPoco>();
                Assert.True(results.Count() >= entities.Count);
            }
        }

        #endregion

        #region async

        public async void AddAsync()
        {
            var myPoco = CreatePoco();
            int result;
            using (var conn = _connFunc())
            {
                result = await conn.AddAsync(myPoco);
            }

            Assert.Equal(1, result);
        }

        public async void AddRangeAsync()
        {
            const int quantity = 10;
            var myPocos = CreatePocos(quantity);
            int result;
            using (var conn = _connFunc())
            {
                result = await conn.AddRangeAsync(myPocos);
            }

            Assert.Equal(quantity, result);
        }

        public async void RemoveByIdAsync()
        {
            int result;
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                await conn.AddAsync(entity);
                result = await conn.RemoveAsync<MyPoco>(entity.Id);
            }

            Assert.Equal(1, result);
        }

        public async void RemoveByEntityAsync()
        {
            int result;
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                await conn.AddAsync(entity);
                result = await conn.RemoveAsync(entity);
            }

            Assert.Equal(1, result);
        }

        public async void RemoveAllByIdsAsync()
        {
            int result;
            List<MyPoco> entities;
            using (var conn = _connFunc())
            {
                entities = CreatePocos(10);
                await conn.AddRangeAsync(entities);
                result = await conn.RemoveAllAsync<MyPoco>(entities.Select(entity => entity.Id).ToList());
            }

            Assert.Equal(entities.Count, result);
        }

        public async void RemoveAllByEntitiesAsync()
        {
            int result;
            List<MyPoco> entities;
            using (var conn = _connFunc())
            {
                entities = CreatePocos(10);
                await conn.AddRangeAsync(entities);
                result = await conn.RemoveAllAsync(entities);
            }

            Assert.Equal(entities.Count, result);
        }

        public async void RemoveAllAsync()
        {
            int quantity, result;
            using (var conn = _connFunc())
            {
                quantity = (await conn.QueryAsync<MyPoco>()).Count();
                result = await conn.RemoveAllAsync<MyPoco>();
            }

            Assert.Equal(quantity, result);
        }

        public async void UpdateAsync()
        {
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                await conn.AddAsync(entity);
                entity.Name = "hahahahaha";
                var modifyQuantity = await conn.UpdateAsync(entity);
                Assert.Equal(1, modifyQuantity);
                var result = await conn.FirstOrDefaultAsync<MyPoco>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        public async void UpdateAllAsync()
        {
            using (var conn = _connFunc())
            {
                var entities = CreatePocos(10);
                await conn.AddRangeAsync(entities);
                entities.ForEach(entity => entity.Name = "hahahahaha");
                var modifyQuantity = await conn.UpdateAllAsync(entities);
                Assert.Equal(modifyQuantity, entities.Count);
                var results = await conn.QueryAsync<MyPoco>(entities.Select(entity => entity.Id).ToList());
                Assert.Equal(entities.OrderBy(e => e.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"),
                    results.OrderBy(r => r.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"));
            }
        }

        public async void FirstAsync()
        {
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                await conn.AddAsync(entity);
                var result = await conn.FirstAsync<MyPoco>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        public async void SingleAsync()
        {
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                await conn.AddAsync(entity);
                var result = await conn.SingleAsync<MyPoco>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        public async void FirstOrDefaultAsync()
        {
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                await conn.AddAsync(entity);
                var result = await conn.FirstOrDefaultAsync<MyPoco>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        public async void SingleOrDefaultAsync()
        {
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                await conn.AddAsync(entity);
                var result = await conn.SingleOrDefaultAsync<MyPoco>(entity.Id);
                var firstJson = entity.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                var secondJson = result.ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss");
                Assert.Equal(firstJson, secondJson);
            }
        }

        public async void QueryAsync()
        {
            using (var conn = _connFunc())
            {
                var entities = CreatePocos(10);
                await conn.AddRangeAsync(entities);
                var results = await conn.QueryAsync<MyPoco>(entities.Select(e => e.Id).ToList());
                Assert.Equal(entities.OrderBy(e => e.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"),
                    results.OrderBy(r => r.Id).ToJson(dateTimeFormat: "yyyy/MM/dd HH:mm:ss"));
            }
        }

        public async void AllAsync()
        {
            using (var conn = _connFunc())
            {
                var entities = CreatePocos(10);
                await conn.AddRangeAsync(entities);
                var results = await conn.QueryAsync<MyPoco>();
                Assert.True(results.Count() >= entities.Count);
            }
        }

        #endregion

        private static MyPoco CreatePoco(SequentialGuidHelper.SequentialGuidType? guidType = null)
        {
            var m = new Random().Next();
            return new MyPoco
            {
                Id = guidType == null ? Guid.NewGuid() : SequentialGuidHelper.GenerateComb(guidType.Value),
                Name = m % 3 == 0 ? "apple" : m % 2 == 0 ? "banana" : "pear",
                Gender = m % 2 == 0 ? Gender.Male : Gender.Female,
                Birthday = DateTime.Now,
                CreateTime = DateTime.UtcNow,
                Nodes = new List<MyPoco>
                {
                    new MyPoco
                    {
                        Id = guidType == null ? Guid.NewGuid() : SequentialGuidHelper.GenerateComb(guidType.Value),
                        Name = m % 3 == 0 ? "apple" : m % 2 == 0 ? "banana" : "pear",
                        Gender = m % 2 == 0 ? Gender.Male : Gender.Female,
                        Birthday = DateTime.Now,
                        CreateTime = DateTime.UtcNow
                    }
                },
                Kids = new List<MySubPoco>
                {
                    new MySubPoco
                    {
                        Id = guidType == null ? Guid.NewGuid() : SequentialGuidHelper.GenerateComb(guidType.Value),
                        Name = m % 3 == 0 ? "apple" : m % 2 == 0 ? "banana" : "pear",
                        Remark = "This is a sub poco."
                    }
                }
            };
        }

        private static List<MyPoco> CreatePocos(int quantity,
            SequentialGuidHelper.SequentialGuidType? guidType = null)
        {
            return Enumerable.Range(0, quantity).Select(p => CreatePoco(guidType)).ToList();
        }
    }
}