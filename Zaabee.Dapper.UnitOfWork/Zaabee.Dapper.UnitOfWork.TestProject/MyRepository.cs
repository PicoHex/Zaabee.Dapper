using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace Zaabee.Dapper.UnitOfWork.TestProject
{
    public class MyRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public MyRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public MyDomainObject Get(string sql, object param)
        {
            return _unitOfWork.Connection.Query(sql, param, _unitOfWork.Transaction).FirstOrDefault();
        }

        public void Insert(MyDomainObject myPoco)
        {
            var sql =
                $"INSERT INTO my_poco (id,name,gender,birthday,create_time) VALUES (@Id,@Name,@Gender,@Birthday,@CreateTime)";
            _unitOfWork.Connection.Execute(sql, myPoco, _unitOfWork.Transaction);
        }

        public void Insert(List<MyDomainObject> myPocos)
        {
            var sql =
                $"INSERT INTO my_poco (id,name,gender,birthday,create_time) VALUES (@Id,@Name,@Gender,@Birthday,@CreateTime)";
            _unitOfWork.Connection.Execute(sql, myPocos, _unitOfWork.Transaction);
        }

        public void DeleteAll()
        {
            var sql = $"DELETE FROM my_poco";
            _unitOfWork.Connection.Execute(sql, null, _unitOfWork.Transaction);
        }
    }
}