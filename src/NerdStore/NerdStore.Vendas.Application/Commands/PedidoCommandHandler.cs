using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Application.Extensions;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler :
        IRequestHandler<AdicionarItemPedidoCommand, bool>,
        IRequestHandler<AtualizarItemPedidoCommand, bool>,
        IRequestHandler<RemoverItemPedidoCommand, bool>,
        IRequestHandler<AplicarVoucherPedidoCommand, bool>

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


        #region [ Adicionar Item ]

        public async Task<bool> Handle(AdicionarItemPedidoCommand command, CancellationToken cancellationToken)
        {
            if (OCommandEstaInvalido(command)) return false;

            var pedido = await BuscarPedido(command);
            var pedidoItem = new PedidoItem(command.ProdutoId, command.Nome, command.Quantidade, command.ValorUnitario);
            AdicionarItem(pedido, pedidoItem);

            pedido.AdicionarEvento(new PedidoAtualizadoEvent(command.ClienteId, pedido.Id, pedido.ValorTotal));
            var adicionou = await _pedidoRepository.UnitOfWork.Commit();

            return adicionou;
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

        #endregion

        #region [ Atualizar Item ]

        public async Task<bool> Handle(AtualizarItemPedidoCommand command, CancellationToken cancellationToken)
        {
            if (OCommandEstaInvalido(command)) return false;

            var pedido = await BuscarPedido(command.ClienteId);
            if (pedido == null)
                return false;

            var item = await _pedidoRepository.BuscarItemPorPedido(command.PedidoId, command.ProdutoId);
            if (!pedido.Contem(item))
            {
                await _mediatrHandler.PublicarNotificacao(NotificaoDeItemNaoEncontrado);
                return false;
            }

            pedido.AtualizarItem(command.ProdutoId, command.Quantidade);

            // ChangerTracker do EF vai atualizar o item também
            _pedidoRepository.Atualizar(pedido);

            var atualizou = await EfetuaCommitDeAtualizacao(pedido, command.ConverterParaEvento());
            return atualizou;
        }

        #endregion

        #region [ Remover Item ]

        public async Task<bool> Handle(RemoverItemPedidoCommand command, CancellationToken cancellationToken)
        {
            if (OCommandEstaInvalido(command)) return false;

            var pedido = await BuscarPedido(command.ClienteId);
            if (pedido == null)
                return false;

            if (!pedido.Contem(command.ProdutoId))
            {
                await _mediatrHandler.PublicarNotificacao(NotificaoDeItemNaoEncontrado);
                return false;
            }

            pedido.RemoverItem(command.ProdutoId);

            var atualizou = await EfetuaCommitDeAtualizacao(pedido, command.ConverterParaEvento());
            return atualizou;
        }

        #endregion

        #region [ Aplicar Voucher ]

        public async Task<bool> Handle(AplicarVoucherPedidoCommand command, CancellationToken cancellationToken)
        {
            if (OCommandEstaInvalido(command)) return false;

            var pedido = await BuscarPedido(command.ClienteId);
            if (pedido == null)
                return false;

            var aplicouVoucher = await TentaAplicarVoucher(pedido, command.CodigoVoucher);
            if (!aplicouVoucher) return false;

            var atualizou = await EfetuaCommitDeAtualizacao(pedido, command.ConverterParaEvento());
            return atualizou;
        }

        private async Task<bool> TentaAplicarVoucher(Pedido pedido, string codigoVoucher)
        {
            var voucher = await _pedidoRepository.BuscarVoucherPorCodigo(codigoVoucher);
            if (voucher == null)
            {
                await _mediatrHandler.PublicarNotificacao(NotificaoDeVoucherNaoEncontrado);
                return false;
            }

            var validacaoVoucher = pedido.AplicarVoucher(voucher);
            if (!validacaoVoucher.IsValid)
            {
                foreach (var error in validacaoVoucher.Errors)
                    await _mediatrHandler.PublicarNotificacao(
                        new DomainNotificaton(error.ErrorCode, error.ErrorMessage));

                return false;
            }

            return true;
        }

        #endregion


        public async Task<Pedido> BuscarPedido(Guid clienteId)
        {
            var pedido = await _pedidoRepository.BuscarPedidoRascunhoPorClienteId(clienteId);
            if (pedido == null)
            {
                await _mediatrHandler.PublicarNotificacao(NotificaoDePedidoEncontrado);
                return null;
            }

            return pedido;
        }

        private static DomainNotificaton NotificaoDeItemNaoEncontrado =>
            new("Pedido",
                "Item do pedido não encontrado");

        private static DomainNotificaton NotificaoDePedidoEncontrado =>
            new("Pedido",
                "Pedido não encontrado");

        private static DomainNotificaton NotificaoDePedidoNaoAtualizado =>
            new("Pedido",
                "Não foi possível atualizar o pedido");

        private static DomainNotificaton NotificaoDeVoucherNaoEncontrado =>
            new("Pedido",
                "Voucher não encontrado!");

        private async Task<bool> EfetuaCommitDeAtualizacao(Pedido pedido, Event evento)
        {
            var atualizou = await _pedidoRepository.UnitOfWork.Commit();

            if (!atualizou)
                await _mediatrHandler.PublicarNotificacao(NotificaoDePedidoNaoAtualizado);
            else
                pedido.AdicionarEvento(evento);

            return atualizou;
        }

        private bool OCommandEstaInvalido(Command message)
        {
            var comandoInvalido = LancarEventosSeOuverErros(message);
            if (comandoInvalido) return true;
            return false;
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