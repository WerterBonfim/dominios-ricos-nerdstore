using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Messages;

namespace NerdStore.Catalogo.Domain
{
    public class Dimensoes : LidarComValidacoes
    {
        public decimal Altura { get; private set; }
        public decimal Largura { get; private set; }
        public decimal Profundidade { get; private set; }

        public Dimensoes(decimal altura, decimal largura, decimal profundidade)
        {
            Altura = altura;
            Largura = largura;
            Profundidade = profundidade;
            Validar();
        }

        public override void Validar()
        {
            ResultadoDaValidacao = new Validacao().Validate(this);
        }

        public string DescricaoFormatada()
        {
            return $"LxAxP: {Largura} x {Altura} x {Profundidade}";
        }

        public override string ToString()
        {
            return DescricaoFormatada();
        }

        public class Validacao : AbstractValidator<Dimensoes>
        {
            public const string MsgErroAltura = "O campo Altura";
            public const string MsgErroLargura = "O campo Altura";
            public const string MsgErroProfundidade = "O campo Altura";

            public Validacao()
            {
                RuleFor(x => x.Altura)
                    .GreaterThan(0)
                    .WithMessage(MsgErroAltura);
                
                RuleFor(x => x.Largura)
                    .GreaterThan(0)
                    .WithMessage(MsgErroLargura);
                
                RuleFor(x => x.Profundidade)
                    .GreaterThan(0)
                    .WithMessage(MsgErroProfundidade);
            }
        }
    }
}