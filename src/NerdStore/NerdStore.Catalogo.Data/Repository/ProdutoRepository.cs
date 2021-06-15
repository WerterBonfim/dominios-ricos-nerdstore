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
    public class ProdutoRepository : RepositoryBase<Produto>, IProdutoRepository
    {
        private readonly CatalogoContext _context;
        
        public ProdutoRepository(CatalogoContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> ListarCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterPorCategoria(int codigo)
        {
            return await base
                .ListarAsync(
                    x => x.Categoria.Codigo == codigo,
                    x => x.Include(y => y.Categoria));
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