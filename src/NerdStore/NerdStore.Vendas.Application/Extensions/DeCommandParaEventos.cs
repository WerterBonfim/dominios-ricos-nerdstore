using System;
using NerdStore.Core.DomainObjects.DTO;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Commands.Pedido;
using NerdStore.Vendas.Application.Events;

namespace NerdStore.Vendas.Application.Extensions
{
    public static class DeCommandParaEventos
    {
        public static ItemDoPedidoAtualizadoEvent ConverterParaEvento(this AtualizarItemPedidoCommand command)
        {
            return new(
                command.ClienteId,
                command.ProdutoId,
                command.Quantidade);
        }

        public static ProdutoRemovidoDoPedidoEvent ConverterParaEvento(this RemoverItemPedidoCommand command)
        {
            return new(
                command.ClienteId,
                command.ProdutoId
            );
        }

        public static VoucherAplicadoNoPedidoEvent ConverterParaEvento(
            this AplicarVoucherPedidoCommand command,
            Guid voucherId,
            Guid pedidoId)
        {
            return new(
                command.ClienteId,
                pedidoId,
                voucherId
            );
        }

        public static PedidoIniciadoEvent ConverterParaEvento( this FecharPedidoCommand command, ItensDoPedido itensDoPedido)
        {
            return new(
                command.PedidoId,
                command.ClienteId,
                command.Total,
                itensDoPedido,
                command.NomeCartao,
                command.NumeroCartao,
                command.ExpiracaoCartao,
                command.CvvCartao
            );
        }
    }
}