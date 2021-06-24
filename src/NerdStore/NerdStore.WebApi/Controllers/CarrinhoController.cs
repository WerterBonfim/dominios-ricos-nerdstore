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
using NerdStore.Vendas.Application.Queries;

namespace NerdStore.WebApi.Controllers
{
    [Route("carrinho")]
    public class CarrinhoController : BaseController
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IMediatrHandler _mediatrHandler;
        private readonly IPedidoQueries _pedidoQueries;


        public CarrinhoController(
            INotificationHandler<DomainNotificaton> domainNotificationHandler,
            IProdutoAppService produtoAppService,
            IPedidoQueries pedidoQueries,
            IMediatrHandler mediatrHandler) : base(domainNotificationHandler, mediatrHandler)
        {
            _produtoAppService = produtoAppService;
            _mediatrHandler = mediatrHandler;
            _pedidoQueries = pedidoQueries;
        }

        [HttpGet("meu-carrinho")]
        public async Task<IActionResult> MeuCarrinho()
        {
            var viewModel = await _pedidoQueries
                .BuscarCarrinhoCliente(ClienteId);

            return RespostaPersonalizada(viewModel);
        }

        // [HttpPost("aplicar-voucher")]
        // public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
        // {
        //     var command = new AplicarVoucherNoPedidoCommand(ClienteId, voucherCodigo);
        //     var aplicou = await _mediatrHandler.EnviarComando(command);
        //
        //     if (!aplicou)
        //     {
        //         AdicionarErro("Não foi possivel aplicar o voucher.");
        //         return RespostaPersonalizada();
        //     }
        //
        //     var carrinhoViewModel = _pedidoQueries.BuscarCarrinhoCliente(ClienteId);
        //     return RespostaPersonalizada(carrinhoViewModel);
        // }

        // [HttpPost("remover-item")]
        // public async Task<IActionResult> RemoverItem(Guid produtoId)
        // {
        //     var produto = _produtoAppService.BuscarPorId(produtoId);
        //     AdicionarErro("Produto não existe no carrinho");
        //     if (produto == null) return RespostaPersonalizada();
        //
        //     var command = new RemoverItemPedidoCommand(ClienteId, produtoId);
        //     var removido = await _mediatrHandler.EnviarComando(command);
        //
        //     if (!removido)
        //     {
        //         AdicionarErro("Não foi possivel remover o item");
        //         return RespostaPersonalizada();
        //     }
        //     
        //     var carrinhoViewModel = _pedidoQueries.BuscarCarrinhoCliente(ClienteId);
        //     return RespostaPersonalizada(carrinhoViewModel);
        //
        // }

        // [HttpPost("atualizar-item")]
        // public async Task<IActionResult> Atualizar(Guid produtoId, int quantidade)
        // {
        //     var produto = _produtoAppService.BuscarPorId(produtoId);
        //     AdicionarErro("Produto não existe no carrinho");
        //     if (produto == null) return RespostaPersonalizada();
        //
        //     var command = new AtualizarItemPedidoCommand(ClienteId, produtoId, quantidades);
        //     var atualizado = await _mediatrHandler.EnviarComando(command);
        //
        //     if (!atualizado)
        //     {
        //         AdicionarErro("Não foi possivel atualizar o carrinho");
        //         return RespostaPersonalizada();
        //     }
        //     
        //     var carrinhoViewModel = _pedidoQueries.BuscarCarrinhoCliente(ClienteId);
        //     return RespostaPersonalizada(carrinhoViewModel);
        //
        // }


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