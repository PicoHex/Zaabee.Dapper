using System.Data;

namespace Zaabee.UnitOfWork
{
    public class UnitOfWork
    {
        public IDbConnection Connection { get; private set; }
        public IDbTransaction Transaction { get; private set; }

        internal UnitOfWork(IDbConnection connection)
        {
            Connection = connection;
            Transaction = Connection.BeginTransaction();
        }

        public void BeginTransaction() => Transaction = Connection.BeginTransaction();

        public void BeginTransaction(IsolationLevel il) => Transaction = Connection.BeginTransaction(il);

        public void UseTransaction(IDbTransaction transaction)
        {
            Transaction = transaction;
            Connection = transaction.Connection;
        }

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
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();
        }
    }
}