using System;
using System.Data;

namespace Zaabee.Dapper.UnitOfWork
{
    public class DalSession : IDisposable
    {
        private readonly IDbConnection _connection;

        public DalSession(IDbConnection dbConnection)
        {
            _connection = dbConnection;
            _connection.Open();
            UnitOfWork = new UnitOfWork(_connection);
        }

        public UnitOfWork UnitOfWork { get; }

        public void Dispose()
        {
            UnitOfWork.Dispose();
            _connection.Dispose();
        }
    }
}