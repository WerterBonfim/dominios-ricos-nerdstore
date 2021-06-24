using System;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Events
{
    public class ItemDoPedidoAtualizadoEvent : Event
    {
        public Guid ClienteId { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid PedidoId { get; set; }
        public int Quantidade { get; set; }

        public ItemDoPedidoAtualizadoEvent(Guid clienteId, Guid produtoId, Guid pedidoId, int quantidade)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            PedidoId = pedidoId;
            Quantidade = quantidade;
        }
    }
}