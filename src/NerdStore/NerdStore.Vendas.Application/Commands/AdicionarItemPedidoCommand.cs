using System;
using FluentValidation;
using FluentValidation.Results;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Commands
{
    public class AdicionarItemPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; set; }
        public string Nome { get; private set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public AdicionarItemPedidoCommand(Guid clienteId, Guid produtoId, string nome, int quantidade, decimal valorUnitario)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Nome = nome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            Validar();
        }

        public override void Validar()
        {
            ResultadoDaValidacao = new Validacao().Validate(this);
        }


        protected class Validacao : AbstractValidator<AdicionarItemPedidoCommand>
        {
            public static string IdClienteErroMsg => "Id do cliente inválido";
            public static string IdProdutoErroMsg => "Id do produto inválido";
            public static string NomeErroMsg => "O nome do produto não foi informado";
            public static string QtdMaxErroMsg => $"A quantidade máxima de um item é {Pedido.QUANTIDADE_MAXIMA_ITENS}";
            public static string QtdMinErroMsg => "A quantidade miníma de um item é 1";
            public static string ValorErroMsg => "O valor do item precisa ser maior que 0";
            
            public Validacao()
            {
                RuleFor(c => c.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage(IdClienteErroMsg);

                RuleFor(c => c.ProdutoId)
                    .NotEqual(Guid.Empty)
                    .WithMessage(IdProdutoErroMsg);

                RuleFor(c => c.Nome)
                    .NotEmpty()
                    .WithMessage(NomeErroMsg);

                RuleFor(c => c.Quantidade)
                    .GreaterThan(0)
                    .WithMessage(QtdMinErroMsg)
                    .LessThanOrEqualTo(Pedido.QUANTIDADE_MAXIMA_ITENS)
                    .WithMessage(QtdMaxErroMsg);

                RuleFor(c => c.ValorUnitario)
                    .GreaterThan(0)
                    .WithMessage(ValorErroMsg);
            }
        }
    }
}