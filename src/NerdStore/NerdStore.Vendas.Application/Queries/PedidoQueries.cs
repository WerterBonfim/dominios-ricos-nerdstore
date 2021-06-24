using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NerdStore.Vendas.Application.Queries.ViewModels;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Queries
{
    public class PedidoQueries : IPedidoQueries
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoQueries(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<CarrinhoViewModel> BuscarCarrinhoCliente(Guid clienteId)
        {
            var pedido = await _pedidoRepository.BuscarPedidoRascunhoPorClienteId(clienteId);
            if (pedido == null) return null;

            var carrinho = MontarCarrinho(pedido);

            return carrinho;
        }
        private static CarrinhoViewModel MontarCarrinho(Pedido pedido)
        {
            var carrinho = new CarrinhoViewModel
            {
                ClienteId = pedido.ClienteId,
                ValorTotal = pedido.ValorTotal,
                PedidoId = pedido.Id,
                ValorDesconto = pedido.ValorDeDesconto
            };
            
            carrinho.Itens = MontarItensDoCarrinho(pedido);
            
            if (pedido.VoucherId != null) 
                carrinho.VoucherCodigo = pedido.Voucher.Codigo;
            
            return carrinho;
        }
        private static List<ItemDoCarrinhoViewModel> MontarItensDoCarrinho(Pedido pedido)
        {
            var itens = new List<ItemDoCarrinhoViewModel>();
            foreach (var item in pedido.Itens)
            {
                itens.Add(new ItemDoCarrinhoViewModel
                {
                    ProdutoId = item.ProdutoId,
                    NomeDoProduto = item.Titulo,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario,
                    SubTotal = item.ValorUnitario * item.Quantidade
                });
            }

            return itens;
        }

        public async Task<IEnumerable<PedidoViewModel>> BuscarPedidosCliente(Guid clienteId)
        {
            var pedidos = await BuscarHistoricoDePedidos(clienteId);
            if (!pedidos.Any()) return null;

            var pedidosViewModel = new List<PedidoViewModel>();
            foreach (var pedido in pedidos)
                pedidosViewModel.Add(MontarPedidoViewModel(pedido));
            
            return pedidosViewModel;
        }

        private PedidoViewModel MontarPedidoViewModel(Pedido pedido)
        {
            return new()
            {
                ValorTotal = pedido.ValorTotal,
                PedidoStatus = (int) pedido.Status,
                Codigo = pedido.Codigo,
                DataCadastro = pedido.DataCadastro
            };
        }

        private async Task<IEnumerable<Pedido>> BuscarHistoricoDePedidos(Guid clienteId)
        {
            var pedidos = await _pedidoRepository.ListarPorClienteId(clienteId);

            pedidos = pedidos
                .Where(x => x.Status == PedidoStatus.Pago | x.Status == PedidoStatus.Cancelado)
                .OrderByDescending(x => x.Codigo);
            return pedidos;
        }
    }
}