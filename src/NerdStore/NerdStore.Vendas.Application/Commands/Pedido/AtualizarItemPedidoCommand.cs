using System;
using FluentValidation;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands.Pedido
{
    public class AtualizarItemPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public int Quantidade { get; private set; }

        public AtualizarItemPedidoCommand(Guid clienteId, Guid produtoId, int quantidade)
        {
            AggregateId = clienteId;
            ClienteId = clienteId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
            Validar();
        }

        public override void Validar()
        {
            ResultadoDaValidacao = new Validacao().Validate(this);
        }
        
        private class Validacao : AbstractValidator<AtualizarItemPedidoCommand>
        {
            public Validacao()
            {
                RuleFor(x => x.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente é inválido");

                RuleFor(x => x.ProdutoId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do produto é inválido");
                
                RuleFor(x => x.Quantidade)
                    .GreaterThan(1)
                    .WithMessage("A quantidade miníma de item é 1");
                
                RuleFor(x => x.Quantidade)
                    .LessThan(15)
                    .WithMessage("A quantidade maxima de itens é 15");
            }
        }
    }
}