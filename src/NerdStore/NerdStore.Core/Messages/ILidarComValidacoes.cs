using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace NerdStore.Core.Messages
{
    public interface ILidarComValidacoes
    {
        ValidationResult ResultadoDaValidacao { get; set; }
        /// <summary>
        /// Esse metodo deve ser implementado na classe que utiliza AbstractValidator do
        /// FluentValidation.
        /// Deve ser chamado no final do construtor da classe.
        /// <example>ResultadoDaValidacao = new Validacao().Validate(this);</example>
        /// </summary>
        void Validar();
        
        /// <summary>
        /// <example>return ResultadoDaValidacao.IsValid;</example>
        /// </summary>
        /// <returns></returns>
        bool EValido();
        
        /// <summary>
        /// <example>
        /// return ResultadoDaValidacao
        ///     .Errors
        ///     .Select(x => x.ErrorMessage);
        /// </example>
        /// </summary>
        /// <returns>Uma lista de string contendo os erros</returns>
        IEnumerable<string> ListarErros();
        
    }
}