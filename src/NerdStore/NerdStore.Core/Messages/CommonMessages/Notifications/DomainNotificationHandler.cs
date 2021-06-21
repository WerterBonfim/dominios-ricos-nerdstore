using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace NerdStore.Core.Messages.CommonMessages.Notifications
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotificaton>, IDisposable
    {
        private List<DomainNotificaton> _notificatons = new();
        
        
        public Task Handle(DomainNotificaton notification, CancellationToken cancellationToken)
        {
            _notificatons.Add(notification);
            return Task.CompletedTask;
        }

        public virtual List<DomainNotificaton> ObterNotificacoes()
        {
            return _notificatons;
        }

        public virtual bool TemNotificacao() => _notificatons.Any();

        public void Dispose()
        {
            _notificatons = new List<DomainNotificaton>();
        }
    }
}