using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Core.Communication;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Core.WebApi.Controllers;
using NerdStore.Vendas.Application.Commands;

namespace NerdStore.WebApi.Controllers
{
    
    [Route("carrinho")]
    public class CarrinhoController : BaseController
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IMediatrHandler _mediatrHandler;


        public CarrinhoController(
            INotificationHandler<DomainNotificaton> domainNotificationHandler,
            IProdutoAppService produtoAppService, 
            IMediatrHandler mediatrHandler) : base(domainNotificationHandler, mediatrHandler)
        {
            _produtoAppService = produtoAppService;
            _mediatrHandler = mediatrHandler;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="produtoId">werter</param>
        /// <param name="quantidade"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("adicionar-item")]
        public async Task<IActionResult> AdicionarItem(Guid produtoId, int quantidade)
        {
            var produto = await _produtoAppService.BuscarPorId(produtoId);
            if (OProdutoEstaIncosistente(quantidade, produto)) 
                return RespostaPersonalizada();

            var command =
                new AdicionarItemPedidoCommand(
                    new Guid("E1D6E0BA-2C6C-4CFA-B0A9-7D107933B67B"), 
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