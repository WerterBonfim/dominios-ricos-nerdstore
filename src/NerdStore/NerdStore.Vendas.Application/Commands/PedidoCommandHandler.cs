using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler : 
        IRequestHandler<AdicionarItemPedidoCommand, bool>
    {
        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            var comandoInvalido = LancarEventosSeOuverErros(message);
            if (comandoInvalido) return false;



            return true;
        }

        private bool LancarEventosSeOuverErros(Command message)
        {
            if (message.EValido()) return true;

            foreach (string erro in message.ListarErros())
            {
                // lançar um evento de erro
            }

            return false;
        }
    }
}