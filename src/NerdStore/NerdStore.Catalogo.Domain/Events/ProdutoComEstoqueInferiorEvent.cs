using System;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Messages.CommonMessages.DomainEvents;

namespace NerdStore.Catalogo.Domain.Events
{
    public class ProdutoComEstoqueInferiorEvent : DomainEvent
    {
        public int QuantidadeRestante { get; private set; }
        
        public ProdutoComEstoqueInferiorEvent(
            Guid aggregateId,
            int quantidadeRestante
            ) : base(aggregateId)
        {
            QuantidadeRestante = quantidadeRestante;
        }
    }
}