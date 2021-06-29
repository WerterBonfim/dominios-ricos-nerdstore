using System;
using System.Collections.Generic;

namespace NerdStore.Core.DomainObjects.DTO
{
    public class ItensDoPedido
    {
        public Guid PedidoId { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}