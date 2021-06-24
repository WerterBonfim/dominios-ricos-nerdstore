using System;
using FluentValidation;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands
{
    public class RemoverItemPedidoCommand : Command
    {
        public Guid ClienteId { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid PedidoId { get; set; }

        public RemoverItemPedidoCommand(Guid clienteId, Guid produtoId, Guid pedidoId)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            PedidoId = pedidoId;

        }
        
        public override void Validar()
        {
            ResultadoDaValidacao = new Validacao().Validate(this);
        }

        private class Validacao : AbstractValidator<RemoverItemPedidoCommand>
        {
            public Validacao()
            {
                RuleFor(x => x.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente é inválido");


                RuleFor(x => x.ProdutoId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do produto é inválido");
            }
        }
    }
}