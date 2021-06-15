using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Core.Data
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        void Adicionar(T entity);
        void Remover(T entity);
        void Atualizar(T entity);

        Task<T> BuscarPorIdAsync(Guid id);
        Task<T> PrimeiroAsync(Expression<Func<T, bool>> expression);
        Task<int> QuantidadeAsync(Expression<Func<T, bool>> expression);

        Task<List<T>> ListarAsync(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int skip = 0,
            int take = 10
        );
    }
}