using System.Collections.Generic;
using System.Threading.Tasks;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Data;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Messages.CommonMessages.Notifications;

namespace NerdStore.Core.Messages
{
    public abstract class CommandHandlerBase<T> where T : IAggregateRoot
    {
        protected readonly IMediatrHandler MediatrHandler;
        protected readonly IRepository<T> _repository;
        
        public CommandHandlerBase(
            IMediatrHandler mediatrHandler
            )
        {
            MediatrHandler = mediatrHandler;
        }
        
        public bool OCommandEstaInvalido(Command message)
        {
            var comandoInvalido = LancarEventosSeOuverErros(message);
            if (comandoInvalido) return true;
            return false;
        }

        private bool LancarEventosSeOuverErros(Command message)
        {
            if (message.EValido()) return false;

            foreach (var erro in message.ResultadoDaValidacao.Errors)
                MediatrHandler.PublicarNotificacao(new DomainNotificaton(message.MessageType, erro.ErrorMessage));

            return true;
        }
        
        
        public async Task<bool> EfetuaCommitDeAtualizacao(
            Entity entidade, 
            IList<DomainNotificaton> notificacoes,
            IList<Event> eventos
            )
        {
            var atualizou = await _repository.UnitOfWork.Commit();

            if (!atualizou)
            {
                foreach (var notificacao in notificacoes)
                    await MediatrHandler.PublicarNotificacao(notificacao);
                
                return atualizou;
            }

            foreach (var evento in eventos)
                entidade.AdicionarEvento(evento);

            return atualizou;
        }
    }
}