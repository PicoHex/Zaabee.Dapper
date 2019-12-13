using System;
using System.Data;

namespace Zaabee.Dapper.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly Guid _id;
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; private set; }

        internal UnitOfWork(IDbConnection connection)
        {
            _id = Guid.NewGuid();
            Connection = connection;
        }

        Guid IUnitOfWork.Id => _id;

        public void Begin()
        {
            Transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            Transaction.Commit();
            Transaction.Dispose();
        }

        public void Rollback()
        {
            Transaction.Rollback();
            Transaction.Dispose();
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();
        }
    }
}