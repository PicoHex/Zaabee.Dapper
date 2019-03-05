using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
            using (var dalSession = new DalSession(GetPgConn()))
            {
                var unitOfWork = dalSession.UnitOfWork;
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
            using (var dalSession = new DalSession(GetMySqlConn()))
            {
                var myDomainObject= CreateDomainObject();
                //Your database code here
                //UoW have no effect here as Begin() is not called.
                var myRepository = new MyRepository(dalSession.UnitOfWork);
                myRepository.Insert(myDomainObject);
            }
        }

        public void Test3()
        {
            var results = new Dictionary<string, Dictionary<string, List<long>>>();

            for (var i = 0; i < 5; i++)
            {
                for (var guidType = 0; guidType < 4; guidType++)
                {
                    for (var dbType = 0; dbType < 3; dbType++)
                    {
                        using (var dalSession =
                            new DalSession(dbType == 0 ? GetMsSqlConn() : dbType == 1 ? GetMySqlConn() : GetPgConn()))
                        {
                            var unitOfWork = dalSession.UnitOfWork;
                            unitOfWork.Begin();
                            try
                            {
                                var domainObjects = CreateDomainObjects(10000,
                                    guidType == 3 ? null : (SequentialGuidHelper.SequentialGuidType?) guidType);

                                var myRepository = new MyRepository(unitOfWork);

                                var sw = Stopwatch.StartNew();
                                myRepository.Insert(domainObjects);
                                unitOfWork.Commit();
                                sw.Stop();
                                AddResult(results,
                                    guidType == 3 ? "原生GUID" : ((SequentialGuidHelper.SequentialGuidType?) guidType).ToString(),
                                    dbType == 0 ? "MsSql" : dbType == 1 ? "MySql" : "PgSql",
                                    sw.ElapsedMilliseconds);
                            }
                            catch
                            {
                                unitOfWork.Rollback();
                                throw;
                            }
                        }

                        using (var dalSession =
                            new DalSession(dbType == 0 ? GetMsSqlConn() : dbType == 1 ? GetMySqlConn() : GetPgConn()))
                        {
                            var unitOfWork = dalSession.UnitOfWork;
                            unitOfWork.Begin();
                            try
                            {
                                var myRepository = new MyRepository(unitOfWork);
                                myRepository.DeleteAll();
                                unitOfWork.Commit();
                            }
                            catch
                            {
                                unitOfWork.Rollback();
                                throw;
                            } 
                        }
                    }
                }
            }
        }

        private void AddResult(
            Dictionary<string, Dictionary<string, List<long>>> results,
            string guidType,
            string dbType,
            long elapsedMilliseconds)
        {
            results = results ??
                      new Dictionary<string, Dictionary<string, List<long>>>();
            if (!results.ContainsKey(guidType))
                results.Add(guidType, new Dictionary<string, List<long>>());
            var r1 = results[guidType];
            if (!r1.ContainsKey(dbType))
                r1.Add(dbType, new List<long>());
            var r2 = r1[dbType];
            r2.Add(elapsedMilliseconds);
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