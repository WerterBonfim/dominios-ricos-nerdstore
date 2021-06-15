using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler :
        IRequestHandler<AdicionarItemPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoCommandHandler(
            IPedidoRepository pedidoRepository
        )
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            var comandoInvalido = LancarEventosSeOuverErros(message);
            if (comandoInvalido) return false;

            var pedido = await BuscarPedido(message);
            var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);
            AdicionarItem(pedido, pedidoItem);

            var adicionou = await _pedidoRepository.UnitOfWork.Commit();

            return adicionou;
        }

        private void AdicionarItem(Pedido pedido, PedidoItem item)
        {
            var itemJaFoiAdicionadoPreviamente = pedido.Contem(item);
            pedido.AdicionarItem(item);

            if (itemJaFoiAdicionadoPreviamente)
                _pedidoRepository.AtualizarItem(item);
            else
                _pedidoRepository.AdicionarItem(item);
        }

        private async Task<Pedido> BuscarPedido(AdicionarItemPedidoCommand message)
        {
            // TODO: Simular varios pedidos não finalizados 
            var pedido = await _pedidoRepository.BuscarPedidoRascunhoPorClienteId(message.ClienteId);
            if (pedido == null)
            {
                pedido = new Pedido(message.ClienteId);
                _pedidoRepository.Adicionar(pedido);
            }

            return pedido;
        }

        private bool LancarEventosSeOuverErros(Command message)
        {
            if (message.EValido()) return true;

            foreach (string erro in message.ListarErros())
            {
                // lançar um evento de erro
            }

            return false;
        }
    }
}