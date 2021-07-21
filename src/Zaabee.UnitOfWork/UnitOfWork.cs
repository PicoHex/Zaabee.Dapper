using System.Data;

namespace Zaabee.UnitOfWork
{
    public class UnitOfWork
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; }

        internal UnitOfWork(IDbConnection connection)
        {
            Connection = connection;
            Transaction = Connection.BeginTransaction();
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