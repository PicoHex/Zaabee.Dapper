using System;
using System.Data;

namespace Zaabee.Dapper.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public Guid Id { get; }
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; private set; }

        internal UnitOfWork(IDbConnection connection)
        {
            Id = Guid.NewGuid();
            Connection = connection;
        }

        public void Begin() => Transaction = Connection.BeginTransaction();

        public void Commit()
        {
            try
            {
                Transaction.Commit();
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
            finally
            {
                Transaction.Dispose();
            }
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