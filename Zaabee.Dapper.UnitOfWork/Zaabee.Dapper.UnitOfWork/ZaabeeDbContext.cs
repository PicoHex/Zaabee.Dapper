using System.Data;
using Zaabee.Dapper.UnitOfWork.Abstractions;

namespace Zaabee.Dapper.UnitOfWork
{
    public class ZaabeeDbContext : IZaabeeDbContext
    {
        private readonly IDbConnection _connection;

        public IZaabeeUnitOfWork UnitOfWork { get; }

        public ZaabeeDbContext(IDbConnection dbConnection)
        {
            _connection = dbConnection;
            _connection.Open();
            UnitOfWork = new ZaabeeUnitOfWork(_connection);
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
            _connection.Dispose();
        }
    }
}