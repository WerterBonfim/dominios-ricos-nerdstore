using System;
using MediatR;

namespace NerdStore.Core.Messages.CommonMessages.Notifications
{
    public class DomainNotificaton : Message, INotification
    {
        public DateTime Timestap { get; private set; }
        public Guid DomainNotificationId { get; private set; }
        public string Key { get; set; }
        public string Value { get; private set; }
        public int Version { get; private set; }

        public DomainNotificaton(string key, string value)
        {
            Timestap = DateTime.Now;
            DomainNotificationId = Guid.NewGuid();
            Version = 1;
            
            Key = key;
            Value = value;
        }
    }
}