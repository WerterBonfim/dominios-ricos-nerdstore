using System.Collections.Generic;
using System.Threading.Tasks;
using NerdStore.Core.Data;

namespace NerdStore.Catalogo.Domain
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Categoria>> ListarCategorias();
        Task<IEnumerable<Produto>> ObterPorCategoria(int codigo);
        
        void Adicionar(Categoria categoria);
        void Atualizar(Categoria categoria);
    }
}