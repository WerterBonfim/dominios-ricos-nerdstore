using System;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Events
{
    public class VoucherAplicadoNoPedidoEvent : Event
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public string CodigoVoucher { get; private set; }

        public VoucherAplicadoNoPedidoEvent(Guid clienteId, Guid pedidoId, string codigoVoucher)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            CodigoVoucher = codigoVoucher;
        }
    }
}