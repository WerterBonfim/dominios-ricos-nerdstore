using System;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain
{
    public class PedidoItem : Entity
    {  public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; }
        public decimal ValorUnitario { get; }
        public int Quantidade { get; private set; }
        public string Titulo { get; }
        public decimal SubTotal => CalcularSubTotal();
        
        // EF Referencia
        public Pedido Pedido { get; set; }

        public PedidoItem()
        {
            
        }

        public PedidoItem(Guid produtoId, string titulo, int quantidade, decimal valorUnitario)
        {
            ProdutoId = produtoId;
            Titulo = titulo;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            
            if (quantidade < Pedido.QUANTIDADE_MINIMA_ITENS)
            {
                var mensagemDeErro =
                    $"Défict na quantidade minima de unidade. " +
                    $"O minimo é {Pedido.QUANTIDADE_MINIMA_ITENS}";
                throw new DomainException(mensagemDeErro);
            }
        }

        private decimal CalcularSubTotal()
        {
            return Quantidade * ValorUnitario;
        }

        internal void AdicionarQuantidade(int quantidade)
        {
            Quantidade += quantidade;
        }
        public void AtualizarQuantidade(int quantidade)
        {
            Quantidade = quantidade;
        }

        public override void Validar()
        {
            throw new NotImplementedException();
        }

        public void AssociarPedido(Guid id)
        {
            PedidoId = id;
        }
    }
}