using System;
using NerdStore.Core.DomainObjects;

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