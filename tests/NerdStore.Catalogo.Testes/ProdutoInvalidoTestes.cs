using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Common;
using NerdStore.Catalogo.Domain;
using Xunit;

namespace NerdStore.Catalogo.Testes
{
    public class ProdutosInvalidoTestes
    {
        [Fact]
        [Trait("Category", "Inválida")]
        public void DeveNotificarErroTodasPropridadesInvalidas()
        {
            var produto = new Produto(
                "",
                "", 
                true, 
                0, 
                DateTime.Now, 
                "", 
                Guid.Empty, new Dimensoes(0, 0, 0));

            var eValido = produto.EValido();

            eValido
                .Should()
                .BeFalse("Todas as propriedades estão inválidas");


            var listaDeErrosEsperado = new string[]
            {
                Produto.Validacao.MsgErroNomeVazio,
                Produto.Validacao.MsgErroImagemVazio,
                Produto.Validacao.MsgErroDescricaoVazio,
                Produto.Validacao.MsgErroCategoriaIdVazio,
                Produto.Validacao.MsgErroValorMenorOuIgualAZero,
            };

            var erros = produto.ListarErros();
            var teste = erros.Count();

            erros.Count()
                .Should()
                .Be(5);

            erros
                .Should()
                .Contain(listaDeErrosEsperado, "Todas as propriedades estão inválidas");

        }
    }
}