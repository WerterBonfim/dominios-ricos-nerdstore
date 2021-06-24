using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentValidation.Results;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Vendas.Domain
{
    public class Pedido : Entity, IAggregateRoot
    {
        public const int QUANTIDADE_MAXIMA_ITENS = 15;
        public const int QUANTIDADE_MINIMA_ITENS = 1;
        
        private readonly Collection<PedidoItem> _itens;

        public int Codigo { get; private set; }
        public decimal ValorTotal => ObterValorTotal();
        public Guid? VoucherId { get; set; }
        public Voucher Voucher { get; private set; }
        public Guid ClienteId { get; }
        public PedidoStatus Status { get; private set; }
        public bool VoucherUtilizado { get; private set; }
        public decimal ValorDeDesconto { get; private set; }
        public IReadOnlyCollection<PedidoItem> Itens => _itens;
        

        public Pedido()
        {
            
        }

        public Pedido(Guid clienteId)
        {
            ClienteId = clienteId;
            _itens = new Collection<PedidoItem>();
            Status = PedidoStatus.Rascunho;
        }

        public void AdicionarItem(PedidoItem item)
        {
            item.AssociarPedido(Id);

            var produtoJaFoiAdicionado = _itens.Contains(item);

            if (produtoJaFoiAdicionado)
                AcrecentarQuantidade(item);
            else
                _itens.Add(item);
        }

        private void AcrecentarQuantidade(PedidoItem item)
        {
            var mesmoItem = _itens.First(x => x.Equals(item));
            mesmoItem.AdicionarQuantidade(item.Quantidade);

            ValidaQuantidadeDeItens(mesmoItem.Quantidade);
        }

        private static void ValidaQuantidadeDeItens(int quantidade)
        {
            if (quantidade < QUANTIDADE_MINIMA_ITENS)
            {
                var mensagemDeErro =
                    $"Défict na quantidade minima de unidade. " +
                    $"O minimo é {Pedido.QUANTIDADE_MINIMA_ITENS}";
                throw new DomainException(mensagemDeErro);
            }

            if (quantidade > QUANTIDADE_MAXIMA_ITENS)
            {
                var mensagemDeErro =
                    $"Excedeu a quantidade maxima de unidade. " +
                    $"O maximo é {Pedido.QUANTIDADE_MAXIMA_ITENS}";

                throw new DomainException(mensagemDeErro);
            }
        }

        private decimal ObterValorTotal()
        {
            var valor = _itens
                .Sum(x => x.SubTotal);

            if (Voucher != null)
                valor = CalcularDescontoDoVoucher(valor);

            return valor;
        }

        private decimal CalcularDescontoDoVoucher(decimal totalPedido)
        {
            if (Voucher.TipoDesconto == TipoDescontoVoucher.Valor)
                return CalculaDescontoPorValor(totalPedido);

            var novoValor = CalculaDescontoPorPorcentagem(totalPedido);

            return novoValor;
        }

        private decimal CalculaDescontoPorPorcentagem(decimal totalPedido)
        {
            if (!Voucher.PercentualDesconto.HasValue)
                return totalPedido;

            ValorDeDesconto = (Voucher.PercentualDesconto.Value / 100) * totalPedido;
            var novoValor = totalPedido - ValorDeDesconto;
            
            if (ValorDeDesconto > totalPedido)
                return 0;
            
            return novoValor;
        }

        private decimal CalculaDescontoPorValor(decimal totalPedido)
        {
            ValorDeDesconto = Voucher.Desconto;
            if (ValorDeDesconto > totalPedido)
                return 0;

            return totalPedido - Voucher.Desconto;
        }

        public void AtualizarItem(Guid produtoId, int quantidade)
        {
            VerificarSeItemExiste(produtoId);
            ValidaQuantidadeDeItens(quantidade);

            var pedidoItem = _itens.First(x => x.ProdutoId == produtoId);
            pedidoItem.AtualizarQuantidade(quantidade);
            
        }

        private void VerificarSeItemExiste(Guid produtoId)
        {
            var existeOItemNoPedido = _itens.Count > 0 && _itens.Any(x => x.ProdutoId == produtoId);
            if (!existeOItemNoPedido)
                throw new DomainException("Não existe esse item no pedido");
        }

        public void RemoverItem(PedidoItem item)
        {
            VerificarSeItemExiste(item.ProdutoId);
            RemoverItem(item.ProdutoId);
        }

        public void RemoverItem(Guid produtoId)
        {
            VerificarSeItemExiste(produtoId);
            var item = _itens.First(x => x.ProdutoId == produtoId);
            _itens.Remove(item);
        }

        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var validacao = voucher.ValidarSeAplicavel();
            if (!validacao.IsValid) return validacao;

            Voucher = voucher;
            VoucherId = voucher.Id;
            VoucherUtilizado = true;

            return validacao;
        }

        public bool Contem(PedidoItem item)
        {
            return _itens.Contains(item);
        }
        
        public bool Contem(Guid produtoId)
        {
            return _itens.Any(x => x.ProdutoId == produtoId);
        }

        public override void Validar()
        {
            
        }
    }
}