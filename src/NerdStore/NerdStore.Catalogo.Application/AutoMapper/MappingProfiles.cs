using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;

namespace NerdStore.Catalogo.Application.AutoMapper
{
    public class DominioParaViewModelMappingProfile : Profile
    {
        public DominioParaViewModelMappingProfile()
        {
            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(d => d.Altura, o => o.MapFrom(s => s.Dimensoes.Altura))
                .ForMember(d => d.Altura, o => o.MapFrom(s => s.Dimensoes.Altura))
                .ForMember(d => d.Altura, o => o.MapFrom(s => s.Dimensoes.Altura));

            CreateMap<Categoria, CategoriaViewModel>();
        }
    }

    public class ViewModelParaDominioMappingProfile : Profile
    {
        public ViewModelParaDominioMappingProfile()
        {
            CreateMap<ProdutoViewModel, Produto>()
                .ConstructUsing(p => new Produto(
                    p.Nome,
                    p.Descricao,
                    p.Ativo, 
                    p.Valor, 
                    p.DataCadastro, 
                    p.Imagem, 
                    p.CategoriaId,
                    new Dimensoes(p.Altura, p.Largura, p.Profundidade)
                ));

            CreateMap<CategoriaViewModel, Categoria>()
                .ConstructUsing(c => new Categoria(c.Nome, c.Codigo));
        }
    }
}