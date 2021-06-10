using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace NerdStore.Core.Messages
{
    public abstract class LidarComValidacoes
    {
        protected ValidationResult ResultadoDaValidacao { get; set; }
        
        public abstract void Validar();

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