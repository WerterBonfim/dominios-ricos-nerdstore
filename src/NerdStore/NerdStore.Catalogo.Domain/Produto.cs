using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain
{
    public class Produto : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string Imagem { get; private set; }
        public int QuantidadeEstoque { get; private set; }
        public Dimensoes Dimensoes { get; private set; }
        
        // EF Relacionamento - ORM
        public Guid CategoriaId { get; private set; }
        public Categoria Categoria { get; private set; }

        public Produto(
            string nome, 
            string descricao, 
            bool ativo, 
            decimal valor, 
            DateTime dataCadastro, 
            string imagem,
            Guid categoriaId
            )
        {
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;
            Valor = valor;
            DataCadastro = dataCadastro;
            Imagem = imagem;
            CategoriaId = categoriaId;
            
            Validar();
            
        }

        //Para o EF
        protected Produto()
        {
            
        }
        
        public void Ativar() => Ativo = true;

        public void Desativar() => Ativo = false;
        
        // Ad hoc Setter
        public void AlterarCategoria(Categoria categoria)
        {
            Categoria = categoria;
            CategoriaId = categoria.Id;
            Validar();
        }

        // Ad hoc Setter
        public void AlterarDescricao(string descricao)
        {
            Descricao = descricao;
            Validar();
        }

        // Ad hoc Setter
        public void DebitarEstoque(int quantidade)
        {
            if (quantidade < 0) quantidade *= -1;
            if (!PossuiEstoque(quantidade)) throw new DomainException("Estoque insuficiente");
            QuantidadeEstoque -= quantidade;
            Validar();
        }

        // Ad hoc Setter
        public void ReporEstoque(int quantidade)
        {
            QuantidadeEstoque += quantidade;
            Validar();
        }

        public bool PossuiEstoque(int quantidade)
        {
            return QuantidadeEstoque >= quantidade;
        }

        public override void Validar()
        {
            ResultadoDaValidacao = new Validacao().Validate(this);
        }


        public sealed class Validacao : AbstractValidator<Produto>
        {
            public const string MsgErroNomeVazio = "O campo Nome do produto não pode sesta vazio";
            public const string MsgErroDescricaoVazio = "O campo Descricao do produto não pode sesta vazio";
            public const string MsgErroImagemVazio = "O campo Imagem do produto não pode sesta vazio";
            public const string MsgErroCategoriaIdVazio = "O campo CategoriaId do produto não pode estar vazio";
            public const string MsgErroValorMenorOuIgualAZero = "O campo Valor do rpduto não pode ser menor ou igual a 0";
            
            public Validacao()
            {
                RuleFor(x => x.Nome)
                    .NotEmpty()
                    .WithMessage(MsgErroNomeVazio);
                
                RuleFor(x => x.Descricao)
                    .NotEmpty()
                    .WithMessage(MsgErroDescricaoVazio);
                
                RuleFor(x => x.Imagem)
                    .NotEmpty()
                    .WithMessage(MsgErroImagemVazio);

                RuleFor(x => x.CategoriaId)
                    .NotEqual(Guid.Empty)
                    .WithMessage(MsgErroCategoriaIdVazio);

                RuleFor(x => x.Valor)
                    .GreaterThan(0)
                    .WithMessage(MsgErroValorMenorOuIgualAZero);
            }
        }

        
    }
}