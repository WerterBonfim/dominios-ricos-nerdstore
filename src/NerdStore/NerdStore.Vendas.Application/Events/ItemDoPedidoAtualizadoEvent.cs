using System;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Events
{
    public class ItemDoPedidoAtualizadoEvent : Event
    {
        public Guid ClienteId { get; set; }
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }

        public ItemDoPedidoAtualizadoEvent(Guid clienteId, Guid produtoId, int quantidade)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }
    }
}