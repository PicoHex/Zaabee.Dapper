using System;
using System.Data;
using Zaabee.Dapper.UnitOfWork.Abstractions;

namespace Zaabee.Dapper.UnitOfWork
{
    public class ZaabeeUnitOfWork : IZaabeeUnitOfWork
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private readonly Guid _id;

        internal ZaabeeUnitOfWork(IDbConnection connection)
        {
            _id = Guid.NewGuid();
            _connection = connection;
        }

        IDbConnection IZaabeeUnitOfWork.Connection => _connection;

        IDbTransaction IZaabeeUnitOfWork.Transaction => _transaction;

        Guid IZaabeeUnitOfWork.Id => _id;

        public void Begin()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }
}