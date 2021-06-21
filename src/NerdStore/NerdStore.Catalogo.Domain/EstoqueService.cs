using System;
using System.Threading.Tasks;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Core.Communication;
using NerdStore.Core.Communication.Mediator;

namespace NerdStore.Catalogo.Domain
{
    // Serviço de domínio.
    // Representa ações conhecidas pela linguagem ubíqua. Todos serviços de 
    // dominios devem ser reconhecida e aprovada pelo Domain Expert
    public class EstoqueService : IEstoqueService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMediatrHandler _bus;

        public EstoqueService(
            IProdutoRepository produtoRepository,
            IMediatrHandler bus
            )
        {
            _produtoRepository = produtoRepository;
            _bus = bus;
        }
        
        public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.BuscarPorIdAsync(produtoId);

            if (!produto.PossuiEstoque(quantidade)) return false;
            
            produto.DebitarEstoque(quantidade);
            
            // TODO: Parametrizar a quantidade de estoque baixo
            if (produto.QuantidadeEstoque < 10)
                await _bus.PublicarEvento(new ProdutoComEstoqueInferiorEvent(produto.Id, produto.QuantidadeEstoque));

            _produtoRepository.Atualizar(produto);

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.BuscarPorIdAsync(produtoId);
            if (produto == null) return false;
            
            produto.ReporEstoque(quantidade);
            
            _produtoRepository.Atualizar(produto);
            
            return await _produtoRepository.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }
    }
}