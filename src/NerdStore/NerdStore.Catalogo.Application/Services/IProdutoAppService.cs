using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NerdStore.Catalogo.Application.ViewModels;

namespace NerdStore.Catalogo.Application.Services
{
    // Atendem a necessidade do frontend
    public interface IProdutoAppService : IDisposable
    {
        Task<IEnumerable<ProdutoViewModel>> ListarPorCategoria(int codigo);
        Task<ProdutoViewModel> BuscarPorId(Guid id);
        Task<IEnumerable<ProdutoViewModel>> Listar();
        Task<IEnumerable<CategoriaViewModel>> ListarCategorias();

        Task AdicionarProduto(ProdutoViewModel produtoViewModel);
        Task AtualizarProduto(ProdutoViewModel produtoViewModel);

        Task<ProdutoViewModel> DebitarEstoque(Guid id, int quantidade);
        Task<ProdutoViewModel> ReporEstoque(Guid id, int quantidade);
    }
}