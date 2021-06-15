using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NerdStore.Core.Data;

namespace NerdStore.Vendas.Domain
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido> BuscarPorId(Guid id);
        Task<IEnumerable<Pedido>> ListarPorClienteId(Guid clienteId);
        Task<Pedido> BuscarPedidoRascunhoPorClienteId(Guid clienteId);

        Task<PedidoItem> BuscarItemPorId(Guid id);
        Task<PedidoItem> BuscarItemPorPedido(Guid pedidoId, Guid produtoId);

        void AdicionarItem(PedidoItem item);
        void AtualizarItem(PedidoItem item);
        void RemoverItem(PedidoItem item);

        Task<Voucher> BuscarVoucherPorCodigo(string codigo);
        
    }
}