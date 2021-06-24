using System;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Events
{
    public class VoucherAplicadoNoPedidoEvent : Event
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid VoucherId { get; private set; }

        public VoucherAplicadoNoPedidoEvent(Guid clienteId, Guid pedidoId, Guid voucherId)
        {
            AggregateId = voucherId;
            VoucherId = voucherId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
        }
    }
}