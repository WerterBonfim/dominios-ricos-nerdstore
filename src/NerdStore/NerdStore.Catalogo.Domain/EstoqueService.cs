using System;
using System.Threading.Tasks;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Core.Communication;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Core.Messages.CommonMessages.Notifications;

namespace NerdStore.Catalogo.Domain
{
    // Serviço de domínio.
    // Representa ações conhecidas pela linguagem ubíqua. Todos serviços de 
    // dominios devem ser reconhecida e aprovada pelo Domain Expert
    public class EstoqueService : IEstoqueService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMediatrHandler _mediatrHandler;

        

        public EstoqueService(
            IProdutoRepository produtoRepository,
            IMediatrHandler mediatrHandler
        )
        {
            _produtoRepository = produtoRepository;
            _mediatrHandler = mediatrHandler;
        }

        public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
        {
            var debitou = await Debitar(produtoId, quantidade);
            if (!debitou) return false;
            
            return await _produtoRepository.UnitOfWork.Commit();
        }

        private async Task<bool> Debitar(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.BuscarPorIdAsync(produtoId);

            if (!produto.PossuiEstoque(quantidade))
            {
                await _mediatrHandler.PublicarNotificacao(new DomainNotificaton("Estoque",
                    $"Produto - {produto.Nome} sem estoque"));

                return false;
            }

            produto.DebitarEstoque(quantidade);

            // TODO: Parametrizar a quantidade de estoque em algum arquivo de configuração
            if (produto.QuantidadeEstoque < 10)
                await _mediatrHandler.PublicarEvento(new ProdutoComEstoqueInferiorEvent(produto.Id, produto.QuantidadeEstoque));

            _produtoRepository.Atualizar(produto);
            return true;
        }

        public async Task<bool> DebitarEstoque(ItensDoPedido itensDoPedido)
        {
            foreach (var item in itensDoPedido.Items)
                await Debitar(item.Id, item.Quantidade);

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
        {
            var repos = await Repor(produtoId, quantidade);
            if (!repos) return false;

            return await _produtoRepository.UnitOfWork.Commit();
        }

        private async Task<bool> Repor(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.BuscarPorIdAsync(produtoId);
            if (produto == null) return false;

            produto.ReporEstoque(quantidade);

            _produtoRepository.Atualizar(produto);
            return true;
        }

        public async Task<bool> ReporEstoque(ItensDoPedido itensDoPedido)
        {
            foreach (var item in itensDoPedido.Items)
                await Repor(item.Id, item.Quantidade);

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }
    }
}