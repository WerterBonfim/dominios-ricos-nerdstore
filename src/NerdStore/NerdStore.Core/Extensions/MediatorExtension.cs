using System.Linq;
using System.Threading.Tasks;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Data;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Core.Extensions
{
    public static class MediatorExtension
    {
        public static async Task PublicarEventos(this IMediatrHandler mediator, DbContextBase ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notificacoes)
                .ToList();
            
            domainEntities.ToList()
                .ForEach(entry => entry.Entity.LimparEventos());

            var tasks = domainEvents
                .Select(async (domainEvent) => await mediator.PublicarEvento(domainEvent));

            await Task.WhenAll(tasks);
        }
    }
}