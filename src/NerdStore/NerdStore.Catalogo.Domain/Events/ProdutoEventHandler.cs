using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;

namespace NerdStore.Catalogo.Domain.Events
{
    public class ProdutoEventHandler :
        INotificationHandler<ProdutoComEstoqueInferiorEvent>,
        INotificationHandler<PedidoIniciadoEvent>
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IMediatrHandler _mediatrHandler;

        public ProdutoEventHandler(
            IProdutoRepository produtoRepository,
            IEstoqueService estoqueService,
            IMediatrHandler mediatrHandler
        )
        {
            _produtoRepository = produtoRepository;
            _estoqueService = estoqueService;
            _mediatrHandler = mediatrHandler;
        }

        public async Task Handle(ProdutoComEstoqueInferiorEvent notification, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.BuscarPorIdAsync(notification.AggregateId);

            // Enviar um email para aquisição de mais produtos.
        }

        public async Task Handle(PedidoIniciadoEvent message, CancellationToken cancellationToken)
        {
            var debitou = await _estoqueService.DebitarEstoque(message.ItensDoPedido);

            if (!debitou)
            {
                await _mediatrHandler.PublicarEvento(new PedidoEstoqueRejeitadoEvent(
                    message.PedidoId,
                    message.ClienteId
                ));
            }


            await _mediatrHandler.PublicarEvento(new PedidoEstoqueConfirmadoEvent(
                message.PedidoId,
                message.ClienteId,
                message.Total,
                message.ItensDoPedido,
                message.NomeCartao,
                message.NumeroCartao,
                message.ExpiracaoCartao,
                message.CvvCartao
            ));
        }
    }
}