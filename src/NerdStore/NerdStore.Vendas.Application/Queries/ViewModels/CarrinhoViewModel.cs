using System;
using System.Collections.Generic;

namespace NerdStore.Vendas.Application.Queries.ViewModels
{
    public class CarrinhoViewModel
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public string VoucherCodigo { get; set; }
        public decimal ValorDesconto { get; set; }
        public List<ItemDoCarrinhoViewModel> Itens { get; set; }
        public PagamentoViaCartaoViewModel Pagamento { get; set; }
    }
}