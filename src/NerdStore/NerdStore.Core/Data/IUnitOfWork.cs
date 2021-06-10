using System;

namespace NerdStore.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
    }
}