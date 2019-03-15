using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Xunit;
using Zaabee.SequentialGuid;

namespace Zaabee.Dapper.UnitOfWork.TestProject
{
    public class UnitTest1
    {

        [Fact]
        public void Test1()
        {
            using (var dbContext = new ZaabeeDbContext(GetPgConn()))
            {
                var unitOfWork = dbContext.UnitOfWork;
                unitOfWork.Begin();
                try
                {
                    var myDomainObject = CreateDomainObject();
                    //Your database code here
                    var myRepository = new MyRepository(unitOfWork);
                    myRepository.Insert(myDomainObject);
                    //You may create other repositories in similar way in same scope of UoW.

                    unitOfWork.Commit();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        [Fact]
        public void Test2()
        {
            using (var dbContext = new ZaabeeDbContext(GetMySqlConn()))
            {
                var myDomainObject= CreateDomainObject();
                //Your database code here
                //UoW have no effect here as Begin() is not called.
                var myRepository = new MyRepository(dbContext.UnitOfWork);
                myRepository.Insert(myDomainObject);
            }
        }

        private IDbConnection GetPgConn()
        {
            return new Npgsql.NpgsqlConnection(
                "Host=192.168.78.152;Username=postgres;Password=123qweasd,./;Database=postgres");
        }

        private IDbConnection GetMySqlConn()
        {
            return new MySql.Data.MySqlClient.MySqlConnection(
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
                CreateTime = DateTime.Now
            };
        }

        private List<MyDomainObject> CreateDomainObjects(int quantity, SequentialGuidHelper.SequentialGuidType? guidType = null)
        {
            return Enumerable.Range(0, quantity).Select(p => CreateDomainObject(guidType)).ToList();
        }
    }
}