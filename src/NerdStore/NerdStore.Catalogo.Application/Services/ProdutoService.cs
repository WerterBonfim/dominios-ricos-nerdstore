using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Application.Services
{
    public class ProdutoService : IProdutoAppService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IMapper _mapper;

        public ProdutoService(
            IProdutoRepository produtoRepository, 
            IMapper mapper,
            IEstoqueService estoqueService)
        {
            _produtoRepository = produtoRepository;
            _estoqueService = estoqueService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterPorCategoria(int codigo)
        {
            var produtosDaCategoria = await _produtoRepository.ObterPorCategoria(codigo);
            var viewModel = _mapper.Map<IEnumerable<ProdutoViewModel>>(produtosDaCategoria);
            return viewModel;
        }

        public async Task<ProdutoViewModel> ObterPorId(Guid id)
        {
            var produto = await _produtoRepository.BuscarPorIdAsync(id);
            var viewModel = _mapper.Map<ProdutoViewModel>(produto);
            return viewModel;
        }

        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            var produtos = await _produtoRepository.ListarAsync();
            var viewModel = _mapper.Map<IEnumerable<ProdutoViewModel>>(produtos);
            return viewModel;
        }

        public async Task<IEnumerable<CategoriaViewModel>> ObterCategorias()
        {
            var categorias = await _produtoRepository.ListarCategorias();
            var viewModel = _mapper.Map<IEnumerable<CategoriaViewModel>>(categorias);
            return viewModel;
        }

        public async Task AdicionarProduto(ProdutoViewModel produtoViewModel)
        {
            var produto = _mapper.Map<Produto>(produtoViewModel);
            _produtoRepository.Adicionar(produto);

            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task AtualizarProduto(ProdutoViewModel produtoViewModel)
        {
            var produto = _mapper.Map<Produto>(produtoViewModel);
            _produtoRepository.Atualizar(produto);
            
            await _produtoRepository.UnitOfWork.Commit();

        }

        public async Task<ProdutoViewModel> DebitarEstoque(Guid id, int quantidade)
        {
            var debitou = await _estoqueService.DebitarEstoque(id, quantidade);
            if (!debitou)
                throw new DomainException("Falha ao debitar estoque");

            return await ObterPorId(id);
        }

        public async Task<ProdutoViewModel> ReporEstoque(Guid id, int quantidade)
        {
            var atualizou = await _estoqueService.ReporEstoque(id, quantidade);
            if (!atualizou)
                throw new DomainException("Falha ao tentar atualizar o estoque");

            return await ObterPorId(id);
        }
        
        public void Dispose()
        {
            _produtoRepository?.Dispose();
            _estoqueService?.Dispose();
        }
    }
}