using System;
using FluentValidation;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands.Pedido
{
    public class FecharPedidoCommand : Command
    {
        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }
        public decimal Total { get; set; }
        public string NomeCartao { get; private set; }
        public string NumeroCartao { get; private set; }
        public string ExpiracaoCartao { get; private set; }
        public string CvvCartao { get; private set; }

        public override void Validar()
        {
            ResultadoDaValidacao = new Validacao().Validate(this);
        }

        public FecharPedidoCommand(
            Guid pedidoId,
            Guid clienteId,
            decimal total,
            string nomeCartao,
            string numeroCartao,
            string cvvCartao)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            Total = total;
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            CvvCartao = cvvCartao;
        }
        
        
        protected class Validacao : AbstractValidator<FecharPedidoCommand>
        {
            public Validacao()
            {
                RuleFor(x => x.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do cliente é inválido");
                
                RuleFor(x => x.PedidoId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do pedido é inválido");

                RuleFor(x => x.NumeroCartao)
                    .NotEmpty()
                    .WithMessage("Número do cartão crédito inválido");

                RuleFor(x => x.NomeCartao)
                    .CreditCard()
                    .WithMessage("O nome do cartão não foi informado");

                RuleFor(x => x.ExpiracaoCartao)
                    .NotEmpty()
                    .WithMessage("Data de expiração não informada");

                RuleFor(x => x.CvvCartao)
                    .Length(3, 4)
                    .WithMessage("O CVV não foi preenchido corretamente");
            }
        }
    }
}