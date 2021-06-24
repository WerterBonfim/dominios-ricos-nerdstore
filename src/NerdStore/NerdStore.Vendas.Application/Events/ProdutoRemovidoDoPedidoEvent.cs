using System;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Events
{
    public class ProdutoRemovidoDoPedidoEvent : Event
    {
        public Guid ClienteId { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid PedidoId { get; set; }

        public ProdutoRemovidoDoPedidoEvent(Guid clienteId, Guid produtoId, Guid pedidoId)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            PedidoId = pedidoId;
        }
    }
}