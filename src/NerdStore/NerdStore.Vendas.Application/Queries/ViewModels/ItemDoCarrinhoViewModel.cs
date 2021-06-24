using System;

namespace NerdStore.Vendas.Application.Queries.ViewModels
{
    public class ItemDoCarrinhoViewModel
    {
        public Guid ProdutoId { get; set; }
        public string NomeDoProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal SubTotal { get; set; }
    }
}