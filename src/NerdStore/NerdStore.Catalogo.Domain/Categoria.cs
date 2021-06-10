using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalogo.Domain
{
    public class Categoria : Entity
    {
        public string Nome { get; private set; }
        public int Codigo { get; private set; }

        // EF Relacionamento
        public ICollection<Produto> Produtos { get; set; }

        protected Categoria() { }

        public Categoria(string nome, int codigo)
        {
            Nome = nome;
            Codigo = codigo;
            Validar();
        }
        
        public override void Validar()
        {
            ResultadoDaValidacao = new Validacao().Validate(this);
        }
        

        public override string ToString()
        {
            return $"{Nome} - {Codigo}";
        }
        
        public class Validacao : AbstractValidator<Categoria>
        {
            public const string MsgErroNome = "O campo Nome da categoria não pode estar vazio";
            public const string MsgErroCodigo = "O campo Código não pode ser menor ou igual a 0";
            
            public Validacao()
            {
                RuleFor(x => x.Nome)
                    .NotEmpty()
                    .WithMessage(MsgErroNome);
                
                RuleFor(x => x.Codigo)
                    .GreaterThan(0)
                    .WithMessage(MsgErroCodigo);
            }
        }
    }
}