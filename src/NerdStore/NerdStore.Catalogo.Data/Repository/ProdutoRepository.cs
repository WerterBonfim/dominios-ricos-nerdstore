using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NerdStore.Catalogo.Domain;
using NerdStore.Core.Data;

namespace NerdStore.Catalogo.Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogoContext _context;
        

        public ProdutoRepository(CatalogoContext context)
        {
            _context = context;
            
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IUnitOfWork UnitOfWork => _context;
        
        public void Adicionar(Produto entity)
        {
            _context.Produtos.Add(entity);
        }

        public void Remover(Produto entity)
        {
            _context.Produtos.Remove(entity);
        }

        public void Atualizar(Produto entity)
        {
            _context.Produtos.Update(entity);
        }

        public async Task<Produto> BuscarPorIdAsync(Guid id)
        {
            return await _context.Produtos
                .FindAsync(id);
        }

        public async Task<Produto> PrimeiroAsync(Expression<Func<Produto, bool>> expression)
        {
            return await _context.Produtos
                .AsNoTrackingWithIdentityResolution()
                .FirstAsync(expression);
        }

        public async Task<int> QuantidadeAsync(Expression<Func<Produto, bool>> expression)
        {
            return await _context.Produtos
                .AsNoTrackingWithIdentityResolution()
                .CountAsync(expression);
        }

        public async Task<List<Produto>> ListarAsync(Expression<Func<Produto, bool>> expression = null, Func<IQueryable<Produto>, IIncludableQueryable<Produto, object>> include = null, int skip = 0, int take = 10)
        {
            var query = _context.Produtos.AsQueryable();

            if (expression != null) query = query.Where(expression);

            if (include != null) query = include(query);

            query = query.Take(take).Skip(skip);

            return await query
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();
        }

        public async Task<IEnumerable<Categoria>> ListarCategorias()
        {
            return await _context.Categorias
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterPorCategoria(int codigo)
        {
            return await _context.Produtos
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Categoria.Codigo == codigo)
                .ToListAsync();
        }

        public void Adicionar(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
        }

        public void Atualizar(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
        }
    }
}