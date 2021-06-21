using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Domain;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Core.WebApi.Controllers;

namespace NerdStore.WebApi.Controllers
{
    [Route("admin")]
    public class AdminController : BaseController
    {
        private readonly IProdutoAppService _produtoAppService;

        public AdminController(IProdutoAppService produtoAppService,
            INotificationHandler<DomainNotificaton> domainNotificationHandler,
            IMediatrHandler mediatrHandler) : base(domainNotificationHandler, mediatrHandler)
        {
            _produtoAppService = produtoAppService;
        }

        [HttpPut("repor-estoque-produto")]
        public async Task<IActionResult> AtualizarEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoAppService.ReporEstoque(produtoId, quantidade);
            return RespostaPersonalizada(produto);
        }
        
        [HttpPut("debitar-estoque-produto")]
        public async Task<IActionResult> DebitarEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoAppService.DebitarEstoque(produtoId, quantidade);
            return RespostaPersonalizada(produto);
        }
        
    }
}