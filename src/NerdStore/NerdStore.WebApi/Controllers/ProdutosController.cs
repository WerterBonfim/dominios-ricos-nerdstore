using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Core.WebApi.Controllers;

namespace NerdStore.WebApi.Controllers
{
    [Authorize]
    [Route("Produtos")]
    public class ProdutosController : BaseController
    {

        private readonly IProdutoAppService _produtoAppService;

        public ProdutosController(IProdutoAppService produtoAppService)
        {
            _produtoAppService = produtoAppService;
        }



        [HttpGet]
        public async Task<IActionResult> ListarProdutos()
        {
            var produtos = await _produtoAppService.Listar();
            return RespostaPersonalizada(produtos);
        }
        
    }
}