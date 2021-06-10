using System.Linq;
using FluentAssertions;
using NerdStore.Catalogo.Domain;
using Xunit;

namespace NerdStore.Catalogo.Testes
{
    public class CategoriaTestes
    {
        [Fact]
        [Trait("Category", "Inválida")]
        public void DeveNotificarErro()
        {
            var categoria = new Categoria("", 0);

            categoria.EValido()
                .Should()
                .BeFalse("Todas as propriedades estão inválidas");

            categoria.ListarErros()
                .Count()
                .Should()
                .Be(2);

            categoria.ListarErros()
                .Should()
                .Contain(new string[]
                {
                    Categoria.Validacao.MsgErroCodigo,
                    Categoria.Validacao.MsgErroNome,
                });
        }
    }
}