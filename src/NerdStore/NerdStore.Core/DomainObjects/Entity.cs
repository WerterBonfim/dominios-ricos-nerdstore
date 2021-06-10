using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using NerdStore.Core.Messages;

namespace NerdStore.Core.DomainObjects
{
    public abstract class Entity : LidarComValidacoes
    {
        public Guid Id { get; private set; }


        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        #region [ Comparações ]
        
        #nullable enable
        public override bool Equals(object? obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 3) + Id.GetHashCode();
        }

        #endregion

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }
    }
}