using System;
using System.Threading.Tasks;
using NerdStore.Core.DomainObjects.DTO;

namespace NerdStore.Catalogo.Domain
{
    public interface IEstoqueService : IDisposable
    {
        Task<bool> DebitarEstoque(Guid produtoId, int quantidade);
        Task<bool> DebitarEstoque(ItensDoPedido itensDoPedido);
        Task<bool> ReporEstoque(Guid produtoId, int quantidade);

        Task<bool> ReporEstoque(ItensDoPedido itensDoPedido);

    }
}