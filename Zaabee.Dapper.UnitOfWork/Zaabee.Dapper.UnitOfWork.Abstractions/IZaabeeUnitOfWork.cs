using System;
using System.Data;

namespace Zaabee.Dapper.UnitOfWork.Abstractions
{
    public interface IZaabeeUnitOfWork : IDisposable
    {
        Guid Id { get; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        void Begin();
        void Commit();
        void Rollback();
    }
}