using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Core.Data
{
    public class RepositoryBase<T> : IRepository<T>, IDisposable where T : class, IAggregateRoot
    {
        private readonly DbSet<T> _dbSet;
        private readonly DbContextBase _context;

        public RepositoryBase(DbContextBase context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Adicionar(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Remover(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Atualizar(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<T> BuscarPorIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> PrimeiroAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public async Task<int> QuantidadeAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.CountAsync(expression);
        }

        public async Task<List<T>> ListarAsync(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int skip = 0,
            int take = 10
        )
        {
            var query = _dbSet.AsQueryable();

            if (expression != null) query = query.Where(expression);

            if (include != null) query = include(query);

            query = query.Take(take).Skip(skip);

            return await query.ToListAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}