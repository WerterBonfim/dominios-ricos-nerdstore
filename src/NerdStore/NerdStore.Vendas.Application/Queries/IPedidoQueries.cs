using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NerdStore.Vendas.Application.Queries.ViewModels;

namespace NerdStore.Vendas.Application.Queries
{
    public interface IPedidoQueries
    {
        Task<CarrinhoViewModel> BuscarCarrinhoCliente(Guid clienteId);
        Task<IEnumerable<PedidoViewModel>> BuscarPedidosCliente(Guid clienteId);
    }
    
}