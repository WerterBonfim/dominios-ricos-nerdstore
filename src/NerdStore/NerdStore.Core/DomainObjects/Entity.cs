using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using NerdStore.Core.Messages;

namespace NerdStore.Core.DomainObjects
{
    public abstract class Entity : ILidarComValidacoes
    {
        public Guid Id { get; private set; }
        public DateTime DataCadastro { get; private set; }
        
        
        private List<Event> _notificacoes;
        public IReadOnlyCollection<Event> Notificacoes => _notificacoes?.AsReadOnly();


        protected Entity()
        {
            Id = Guid.NewGuid();
            DataCadastro = DateTime.Now;
        }

        public void AdicionarEvento(Event evento)
        {
            _notificacoes = _notificacoes ?? new();
            _notificacoes.Add(evento);
        }

        public void RemoverEvento(Event evento) => _notificacoes?.Remove(evento);
        public void LimparEventos() => _notificacoes?.Clear();
        

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

        public ValidationResult ResultadoDaValidacao { get; set; }

        public virtual void Validar()
        {
            throw new NotImplementedException();
        }

        public bool EValido()
        {
            return ResultadoDaValidacao.IsValid;
        }

        public IEnumerable<string> ListarErros()
        {
            return ResultadoDaValidacao
                .Errors
                .Select(x => x.ErrorMessage);
        }
    }
}