using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Core.Bus;
using NerdStore.Core.WebApi.Controllers;
using NerdStore.Vendas.Application.Commands;

namespace NerdStore.WebApi.Controllers
{
    [Authorize]
    [Route("Carrinho")]
    public class CarrinhoController : BaseController
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IMediatrHandler _mediatrHandler;


        public CarrinhoController(IProdutoAppService produtoAppService, IMediatrHandler mediatrHandler)
        {
            _produtoAppService = produtoAppService;
            _mediatrHandler = mediatrHandler;
        }


        [HttpPost]
        [Route("adicionar-item")]
        public async Task<IActionResult> AdicionarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.BuscarPorId(id);
            if (OProdutoEstaIncosistente(quantidade, produto)) 
                return RespostaPersonalizada();

            var command =
                new AdicionarItemPedidoCommand(
                    Guid.NewGuid(), 
                    produto.Id, 
                    produto.Nome, 
                    quantidade, 
                    produto.Valor);
            await _mediatrHandler.EnviarComando(command);
            

            return RespostaDeSucesso("Produto adicionado com sucesso");
        }

        private bool OProdutoEstaIncosistente(int quantidade, ProdutoViewModel produto)
        {
            if (produto == null)
            {
                AdicionarErro("Produto inválido");
                return true;
            }

            if (produto.QuantidadeEstoque < quantidade)
            {
                AdicionarErro("Produto com estque insuficiente");
                return true;
            }

            return false;
        }
    }
}