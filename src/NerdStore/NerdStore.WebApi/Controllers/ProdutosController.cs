using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Core.WebApi.Controllers;

namespace NerdStore.WebApi.Controllers
{
    [Route("produtos")]
    public class ProdutosController : BaseController
    {
        private readonly IProdutoAppService _produtoAppService;

        public ProdutosController(
            IProdutoAppService produtoAppService,
            INotificationHandler<DomainNotificaton> domainNotificationHandler,
            IMediatrHandler mediatrHandler) : base(domainNotificationHandler, mediatrHandler)
            
        {
            _produtoAppService = produtoAppService;
        }


        [HttpGet]
        public async Task<IActionResult> ListarProdutos()
        {
            var produtos = await _produtoAppService.Listar();
            return RespostaPersonalizada(produtos);
        }

        [HttpPost("nova-categoria")]
        public async Task<IActionResult> NovaCategoria(CategoriaViewModel categoria)
        {
            if (!ModelState.IsValid)
                return RespostaDeSucesso("");

            var viewModel = await _produtoAppService.AdicionarCategoria(categoria);
            return RespostaPersonalizada(viewModel);
        }

        [HttpGet("listar-categorias")]
        public async Task<IActionResult> ListarCategorias()
        {
            var viewModel = await _produtoAppService.ListarCategorias();
            return RespostaPersonalizada(viewModel);
        }


        [HttpPost("novo")]
        public async Task<IActionResult> NovoProduto(ProdutoViewModel produto)
        {
            await _produtoAppService.AdicionarProduto(produto);
            return RespostaDeSucesso("Novo produto cadastrado com sucesso");
        }
    }
}