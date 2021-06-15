using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using MediatR;

namespace NerdStore.Core.Messages
{
    public abstract class Command : Message, ILidarComValidacoes, IRequest<bool>
    {
        public DateTime Timestamp { get; set; }

        public Command()
        {
            Timestamp = DateTime.Now;
        }

        public ValidationResult ResultadoDaValidacao { get; set; }

        // Delegando para a classe filha
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