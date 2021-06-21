using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Data.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Codigo)
                .HasDefaultValueSql("NEXT VALUE FOR MinhaSequencia");

            builder.Property(x => x.ClienteId)
                .IsRequired();

            builder.HasMany(c => c.Itens)
                .WithOne(x => x.Pedido)
                .HasForeignKey(c => c.PedidoId);

            builder.ToTable("Pedidos");
        }
    }
}