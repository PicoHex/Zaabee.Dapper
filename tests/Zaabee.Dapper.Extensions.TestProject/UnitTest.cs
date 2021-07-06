using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Xunit;
using Zaabee.Dapper.Extensions.TestProject.POCOs;
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
                result = conn.AddRange<MyPoco>(myPocos);
            }

            Assert.Equal(quantity, result);
        }

        public void DeleteById()
        {
            int result;
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                conn.Add(entity);
                result = conn.DeleteById<MyPoco>(entity.Id);
            }

            Assert.Equal(1, result);
        }

        public void DeleteByEntity()
        {
            int result;
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                conn.Add(entity);
                result = conn.DeleteByEntity(entity);
            }

            Assert.Equal(1, result);
        }

        public void DeleteAllByIds()
        {
            int result;
            List<MyPoco> entities;
            using (var conn = _connFunc())
            {
                entities = CreatePocos(10);
                conn.AddRange<MyPoco>(entities);
                result = conn.DeleteByIds<MyPoco>(entities.Select(entity => entity.Id).ToList());
            }

            Assert.Equal(entities.Count, result);
        }

        public void DeleteAllByEntities()
        {
            int result;
            List<MyPoco> entities;
            using (var conn = _connFunc())
            {
                entities = CreatePocos(10);
                conn.AddRange<MyPoco>(entities);
                result = conn.DeleteByEntities<MyPoco>(entities);
            }

            Assert.Equal(entities.Count, result);
        }

        public void DeleteAll()
        {
            int quantity, result;
            using (var conn = _connFunc())
            {
                quantity = conn.GetAll<MyPoco>().Count();
                result = conn.DeleteAll<MyPoco>();
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
//                var result = conn.FirstOrDefault<MyPoco, MySubPoco, MyPoco>(entity.Id, (mypoco, mySubPoco) =>
//                {
//                    mypoco.Id = mySubPoco.MyPocoId;
//                    return mypoco;
//                });
                var result = conn.FirstOrDefault<MyPoco>(entity.Id);
                var firstJson = entity.ToJson(new JsonSerializerSettings {DateFormatString = "yyyy/MM/dd HH:mm:ss"});
                var secondJson = result.ToJson(new JsonSerializerSettings {DateFormatString = "yyyy/MM/dd HH:mm:ss"});
                Assert.Equal(firstJson, secondJson);
            }
        }

        public void UpdateAll()
        {
            using (var conn = _connFunc())
            {
                var entities = CreatePocos(10);
                conn.AddRange<MyPoco>(entities);
                entities.ForEach(entity => entity.Name = "hahahahaha");
                var modifyQuantity = conn.UpdateAll<MyPoco>(entities);
                Assert.Equal(modifyQuantity, entities.Count);
                var results = conn.Get<MyPoco>(entities.Select(entity => entity.Id).ToList()).ToList();
                Assert.Equal(
                    entities.OrderBy(e => e.Id).ToJson(new JsonSerializerSettings
                        {DateFormatString = "yyyy/MM/dd HH:mm:ss"}),
                    results.OrderBy(r => r.Id).ToJson(new JsonSerializerSettings
                        {DateFormatString = "yyyy/MM/dd HH:mm:ss"}));
            }
        }

        public void FirstOrDefault()
        {
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                conn.Add(entity);
                var result = conn.FirstOrDefault<MyPoco>(entity.Id);
                var firstJson = entity.ToJson(new JsonSerializerSettings {DateFormatString = "yyyy/MM/dd HH:mm:ss"});
                var secondJson = result.ToJson(new JsonSerializerSettings {DateFormatString = "yyyy/MM/dd HH:mm:ss"});
                Assert.Equal(firstJson, secondJson);
            }
        }

        public void Query()
        {
            using (var conn = _connFunc())
            {
                var entities = CreatePocos(10);
                conn.AddRange<MyPoco>(entities);
                var results = conn.Get<MyPoco>(entities.Select(e => e.Id).ToList());
                Assert.Equal(
                    entities.OrderBy(e => e.Id).ToJson(new JsonSerializerSettings
                        {DateFormatString = "yyyy/MM/dd HH:mm:ss"}),
                    results.OrderBy(r => r.Id).ToJson(new JsonSerializerSettings
                        {DateFormatString = "yyyy/MM/dd HH:mm:ss"}));
            }
        }

        public void GetAll()
        {
            using (var conn = _connFunc())
            {
                var entities = CreatePocos(10);
                conn.AddRange<MyPoco>(entities);
                var results = conn.GetAll<MyPoco>();
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

        public async void DeleteByIdAsync()
        {
            int result;
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                await conn.AddAsync(entity);
                result = await conn.DeleteAsync<MyPoco>(entity.Id);
            }

            Assert.Equal(1, result);
        }

        public async void DeleteByEntityAsync()
        {
            int result;
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                await conn.AddAsync(entity);
                result = await conn.DeleteAsync(entity);
            }

            Assert.Equal(1, result);
        }

        public async void DeleteAllByIdsAsync()
        {
            int result;
            List<MyPoco> entities;
            using (var conn = _connFunc())
            {
                entities = CreatePocos(10);
                await conn.AddRangeAsync(entities);
                result = await conn.DeleteAllAsync<MyPoco>(entities.Select(entity => entity.Id).ToList());
            }

            Assert.Equal(entities.Count, result);
        }

        public async void DeleteAllByEntitiesAsync()
        {
            int result;
            List<MyPoco> entities;
            using (var conn = _connFunc())
            {
                entities = CreatePocos(10);
                await conn.AddRangeAsync(entities);
                result = await conn.DeleteAllAsync(entities);
            }

            Assert.Equal(entities.Count, result);
        }

        public async void DeleteAllAsync()
        {
            int quantity, result;
            using (var conn = _connFunc())
            {
                quantity = (await conn.GetAllAsync<MyPoco>()).Count();
                result = await conn.DeleteAllAsync<MyPoco>();
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
                var firstJson = entity.ToJson(new JsonSerializerSettings {DateFormatString = "yyyy/MM/dd HH:mm:ss"});
                var secondJson = result.ToJson(new JsonSerializerSettings {DateFormatString = "yyyy/MM/dd HH:mm:ss"});
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
                var results = await conn.GetAsync<MyPoco>(entities.Select(entity => entity.Id).ToList());
                Assert.Equal(
                    entities.OrderBy(e => e.Id).ToJson(new JsonSerializerSettings
                        {DateFormatString = "yyyy/MM/dd HH:mm:ss"}),
                    results.OrderBy(r => r.Id).ToJson(new JsonSerializerSettings
                        {DateFormatString = "yyyy/MM/dd HH:mm:ss"}));
            }
        }

        public async void FirstOrDefaultAsync()
        {
            using (var conn = _connFunc())
            {
                var entity = CreatePoco();
                await conn.AddAsync(entity);
                var result = await conn.FirstOrDefaultAsync<MyPoco>(entity.Id);
                var firstJson = entity.ToJson(new JsonSerializerSettings {DateFormatString = "yyyy/MM/dd HH:mm:ss"});
                var secondJson = result.ToJson(new JsonSerializerSettings {DateFormatString = "yyyy/MM/dd HH:mm:ss"});
                Assert.Equal(firstJson, secondJson);
            }
        }

        public async void AllAsync()
        {
            using (var conn = _connFunc())
            {
                var entities = CreatePocos(10);
                await conn.AddRangeAsync(entities);
                var results = await conn.GetAllAsync<MyPoco>();
                Assert.True(results.Count() >= entities.Count);
            }
        }

        #endregion

        private static MyPoco CreatePoco(SequentialGuidType? guidType = null)
        {
            var m = new Random().Next();
            var id = guidType is null ? Guid.NewGuid() : SequentialGuidHelper.GenerateComb(guidType.Value);
            return new MyPoco
            {
                Id = id,
                Name = m % 3 is 0 ? "apple" : m % 2 is 0 ? "banana" : "pear",
                Gender = m % 2 is 0 ? Gender.Male : Gender.Female,
                Birthday = DateTime.Now,
                CreateTime = DateTime.UtcNow,
                Kids = new List<MySubPoco>
                {
                    new()
                    {
                        Id = guidType is null ? Guid.NewGuid() : SequentialGuidHelper.GenerateComb(guidType.Value),
                        MyPocoId = id,
                        Name = m % 3 is 0 ? "apple" : m % 2 is 0 ? "banana" : "pear",
                        Remark = "This is a sub poco."
                    }
                }
            };
        }

        private static List<MyPoco> CreatePocos(int quantity, SequentialGuidType? guidType = null)
        {
            return Enumerable.Range(0, quantity).Select(_ => CreatePoco(guidType)).ToList();
        }
    }
}