using System;
using System.Data;

namespace Zaabee.Dapper.UnitOfWork
{
    public interface IUnitOfWork
    {
        Guid Id { get; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        void Begin();
        void Commit();
        void Rollback();
    }
}