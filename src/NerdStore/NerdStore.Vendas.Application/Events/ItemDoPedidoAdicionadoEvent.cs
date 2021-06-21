using System;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Events
{
    public class ItemDoPedidoAdicionadoEvent : Event
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; set; }
        public decimal ValorUnitario { get; private set; }
        public string NomeDoProduto { get; private set; }
        public int Quantidade { get; private set; }


        public ItemDoPedidoAdicionadoEvent(
            Guid clienteId, 
            Guid pedidoId, 
            Guid produtoId,
            string nomeDoProduto,
            decimal valorUnitario, 
            int quantidade)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            NomeDoProduto = nomeDoProduto;
            ValorUnitario = valorUnitario;
            Quantidade = quantidade;
        }
    }
}