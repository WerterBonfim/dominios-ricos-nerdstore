using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Data.Mappings
{
    public class PedidoItemMapping : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Titulo)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(x => x.ProdutoId)
                .IsRequired();
            

            builder.HasOne(x => x.Pedido)
                .WithMany(x => x.Itens);

            builder.ToTable("PedidoItens");

        }
    }
}