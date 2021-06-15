using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NerdStore.Core.Data;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Data.Repository
{
    public class PedidoRepository : RepositoryBase<Pedido>, IPedidoRepository
    {
        private readonly VendasContext _context;

        public PedidoRepository(VendasContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Pedido> BuscarPorId(Guid id)
        {
            return await base.BuscarPorIdAsync(id);
        }

        public async Task<IEnumerable<Pedido>> ListarPorClienteId(Guid clienteId)
        {
            return await base.ListarAsync(x => x.ClienteId == clienteId);
        }

        public async Task<Pedido> BuscarPedidoRascunhoPorClienteId(Guid clienteId)
        {
            Expression<Func<Pedido, bool>> query = x =>
                x.Status == PedidoStatus.Rascunho &&
                x.ClienteId == clienteId;

            var pedido = await _context.Pedidos
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync(query);

            if (pedido == null) return null;
            
            
            
            return await base.PrimeiroAsync(x =>
                x.Status == PedidoStatus.Rascunho &&
                x.ClienteId == clienteId);
        }

        

        public async Task<PedidoItem> BuscarItemPorId(Guid id)
        {
            return await _context.PedidoItens.FindAsync(id);
        }

        public async Task<PedidoItem> BuscarItemPorPedido(Guid pedidoId, Guid produtoId)
        {
            return await _context.PedidoItens
                .FirstOrDefaultAsync(x => x.PedidoId == pedidoId && x.ProdutoId == produtoId);
        }

        public void AdicionarItem(PedidoItem item)
        {
            _context.PedidoItens.Add(item);
        }

        public void AtualizarItem(PedidoItem item)
        {
            _context.PedidoItens.Update(item);
        }

        public void RemoverItem(PedidoItem item)
        {
            _context.PedidoItens.Remove(item);
        }

        public async Task<Voucher> BuscarVoucherPorCodigo(string codigo)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(x => x.Codigo == codigo);
        }
    }
}