using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace NerdStore.Core.WebApi.Controllers
{
    [ApiController]
    public class BaseController : Controller
    {
        private readonly ICollection<string> _erros = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultado"></param>
        /// <returns></returns>
        protected IActionResult RespostaPersonalizada(object resultado = null)
        {
            if (OperacaoValida())
                return Ok(resultado);

            var erros = new Dictionary<string, string[]>
            {
                {"Mensagens", _erros.ToArray()}
            };

            LimparErros();
            return BadRequest(new ValidationProblemDetails(erros));
        }

        protected IActionResult RespostaPersonalizada(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
                AdicionarErro(error.ErrorMessage);

            return RespostaPersonalizada();
        }

        protected IActionResult RespostaDeSucesso(string mensagem)
        {
            return RespostaPersonalizada(new
            {
                mensagem
            });
        }

        protected void AdicionarErro(string mensagem) => _erros.Add(mensagem);
        private bool OperacaoValida() => !_erros.Any();

        private void LimparErros() => _erros.Clear();
    }
}