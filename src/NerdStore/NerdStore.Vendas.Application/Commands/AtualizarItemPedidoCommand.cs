using System;
using FluentValidation;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands
{
    public class AtualizarItemPedidoCommand : Command
    {
        public Guid ClienteId { get; set; }
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }

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
                
                RuleFor(x => x.PedidoId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do pedido é inválido");
                
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