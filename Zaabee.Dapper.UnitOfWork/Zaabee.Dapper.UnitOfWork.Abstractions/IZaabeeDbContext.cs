using System;

namespace Zaabee.Dapper.UnitOfWork.Abstractions
{
    public interface IZaabeeDbContext : IDisposable
    {
        IZaabeeUnitOfWork UnitOfWork { get; }
    }
}