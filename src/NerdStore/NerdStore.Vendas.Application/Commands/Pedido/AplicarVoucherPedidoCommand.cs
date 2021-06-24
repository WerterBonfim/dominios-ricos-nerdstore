using System;
using FluentValidation;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands.Pedido
{
    public class AplicarVoucherPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public string CodigoVoucher { get; private set; }

        public AplicarVoucherPedidoCommand(Guid clienteId, string codigoVoucher)
        {
            AggregateId = clienteId;
            ClienteId = clienteId;
            CodigoVoucher = codigoVoucher;
            Validar();
        }
        
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

                RuleFor(x => x.CodigoVoucher)
                    .NotEmpty()
                    .WithMessage("O código do voucher não pode ser vazio");
            }
        }
    }
}