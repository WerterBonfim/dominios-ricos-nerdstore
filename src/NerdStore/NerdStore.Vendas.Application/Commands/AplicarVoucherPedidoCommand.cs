using System;
using FluentValidation;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands
{
    public class AplicarVoucherPedidoCommand : Command
    {
        public Guid ClienteId { get; set; }
        public Guid PedidoId { get; set; }
        public string CodigoVoucher { get; set; }
        
        public override void Validar()
        {
            ResultadoDaValidacao = new Validacao().Validate(this);
        }
        
        private class Validacao : AbstractValidator<AplicarVoucherPedidoCommand>
        {
            public Validacao()
            {
                RuleFor(x => x.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente é inválido");
                
                RuleFor(x => x.PedidoId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do pedido é inválido");
                
                RuleFor(x => x.CodigoVoucher)
                    .NotEmpty()
                    .WithMessage("O código do voucher não pode ser vazio");
                
               
            }
        }
    }
}