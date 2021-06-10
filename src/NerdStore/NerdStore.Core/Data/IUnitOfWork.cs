using System;
using System.Threading.Tasks;

namespace NerdStore.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}