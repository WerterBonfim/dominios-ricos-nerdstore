using System.Linq;
using FluentAssertions;
using NerdStore.Catalogo.Domain;
using Xunit;

namespace NerdStore.Catalogo.Testes
{
    public class DimencoesTeste
    {
        [Fact]
        [Trait("Category", "Inválida")]
        public void DeveNotificarErroTodasPropriedadesInvalidas()
        {
            var dimecoes = new Dimensoes(0, 0, 0);
            dimecoes
                .EValido()
                .Should()
                .BeFalse("Todas as propriedades estão inválidas");

            dimecoes.ListarErros()
                .Count()
                .Should()
                .Be(3);

            dimecoes.ListarErros()
                .Should()
                .Contain(new string[]
                {
                    Dimensoes.Validacao.MsgErroAltura,
                    Dimensoes.Validacao.MsgErroLargura,
                    Dimensoes.Validacao.MsgErroProfundidade,
                });
        }
        
        [Fact]
        [Trait("Category", "Valido")]
        public void DeveNotificarSucesso()
        {
            var dimecoes = new Dimensoes(1, 1, 1);
            dimecoes
                .EValido()
                .Should()
                .BeTrue();

            dimecoes.ListarErros()
                .Count()
                .Should()
                .Be(0);
        }
        
    }
}