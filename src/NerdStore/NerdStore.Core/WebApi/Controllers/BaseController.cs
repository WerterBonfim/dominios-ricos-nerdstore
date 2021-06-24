using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;

namespace NerdStore.Core.WebApi.Controllers
{
    [ApiController]
    public class BaseController : Controller
    {
        public Guid ClienteId = new Guid("E1D6E0BA-2C6C-4CFA-B0A9-7D107933B67B");
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatrHandler _mediatrHandler;
        private IList<DomainNotificaton> Erros => _notifications.ObterNotificacoes();

        public BaseController(
            INotificationHandler<DomainNotificaton> domainNotificationHandler,
            IMediatrHandler mediatrHandler
            )
        {
            _notifications = (DomainNotificationHandler)domainNotificationHandler;
            _mediatrHandler = mediatrHandler;
        }

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
                {"Mensagens", Erros.Select(x => x.Value).ToArray()}
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

        protected void AdicionarErro(string mensagem) => Erros.Add(new DomainNotificaton("Domain", mensagem));
        private bool OperacaoValida() => !Erros.Any();

        private void LimparErros() => Erros.Clear();
    }
}