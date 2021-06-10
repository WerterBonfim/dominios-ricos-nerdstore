using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace NerdStore.Catalogo.Domain.Events
{
    public class ProdutoEventHandler : INotificationHandler<ProdutoComEstoqueInferiorEvent>
    {
        private readonly IProdutoRepository _produtoRepository;
        
        public async Task Handle(ProdutoComEstoqueInferiorEvent notification, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.BuscarPorIdAsync(notification.AggregateId);
            
            // Enviar um email para aquisição de mais produtos.
            
        }
    }
}