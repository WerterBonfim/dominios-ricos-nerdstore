using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler :
        IRequestHandler<AdicionarItemPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediatrHandler _mediatrHandler;

        public PedidoCommandHandler(
            IPedidoRepository pedidoRepository,
            IMediatrHandler mediatrHandler
        )
        {
            _pedidoRepository = pedidoRepository;
            _mediatrHandler = mediatrHandler;
        }

        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            var comandoInvalido = LancarEventosSeOuverErros(message);
            if (comandoInvalido) return false;

            var pedido = await BuscarPedido(message);
            var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);
            AdicionarItem(pedido, pedidoItem);

            pedido.AdicionarEvento(new PedidoAtualizadoEvent(message.ClienteId, pedido.Id, pedido.ValorTotal));
            var adicionou = await _pedidoRepository.UnitOfWork.Commit();

            return adicionou;
        }

        private void AdicionarItem(Pedido pedido, PedidoItem item)
        {
            var itemJaFoiAdicionadoPreviamente = pedido.Contem(item);
            pedido.AdicionarItem(item);

            if (itemJaFoiAdicionadoPreviamente)
                _pedidoRepository.Atualizar(pedido);
            else
                _pedidoRepository.AdicionarItem(item);

            pedido.AdicionarEvento(new ItemDoPedidoAdicionadoEvent(
                pedido.ClienteId,
                pedido.Id,
                item.ProdutoId,
                item.Titulo,
                item.ValorUnitario,
                item.Quantidade
            ));
        }

        private async Task<Pedido> BuscarPedido(AdicionarItemPedidoCommand message)
        {
            // TODO: Simular varios pedidos não finalizados 
            var pedido = await _pedidoRepository.BuscarPedidoRascunhoPorClienteId(message.ClienteId);
            if (pedido == null)
            {
                pedido = new Pedido(message.ClienteId);
                _pedidoRepository.Adicionar(pedido);
                pedido.AdicionarEvento(new PedidoRascunhoIniciadoEvent(message.ClienteId, pedido.Id));
            }

            return pedido;
        }

        private bool LancarEventosSeOuverErros(Command message)
        {
            if (message.EValido()) return false;

            foreach (var erro in message.ResultadoDaValidacao.Errors)
                _mediatrHandler.PublicarNotificacao(new DomainNotificaton(message.MessageType, erro.ErrorMessage));

            return true;
        }
    }
}