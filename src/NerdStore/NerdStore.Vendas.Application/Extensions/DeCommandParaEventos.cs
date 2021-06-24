using NerdStore.Vendas.Application.Commands;
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
                command.PedidoId,
                command.Quantidade);
        }

        public static ProdutoRemovidoDoPedidoEvent ConverterParaEvento(this RemoverItemPedidoCommand command)
        {
            return new(
                command.ClienteId,
                command.ProdutoId,
                command.PedidoId
            );
        }

        public static VoucherAplicadoNoPedidoEvent ConverterParaEvento(this AplicarVoucherPedidoCommand command)
        {
            return new(
                command.ClienteId,
                command.PedidoId,
                command.CodigoVoucher
            );
        }
    }
}