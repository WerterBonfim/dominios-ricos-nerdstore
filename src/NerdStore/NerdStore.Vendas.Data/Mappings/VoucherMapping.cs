using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Data.Mappings
{
    public class VoucherMapping : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Codigo)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(x => x.Desconto)
                .IsRequired();
            
            builder.Property(x => x.PercentualDesconto)
                .IsRequired();
            
            builder.Property(x => x.Quantidade)
                .IsRequired();
            
            builder.Property(x => x.Validade)
                .IsRequired();
            
            builder.Property(x => x.Ativo);
            
            builder.Property(x => x.Utilizado);
            
            builder.Property(x => x.TipoDesconto);

            builder.HasMany(c => c.Pedidos)
                .WithOne(x => x.Voucher)
                .HasForeignKey(x => x.VoucherId);


            builder.ToTable("Vouchers");
        }
    }
}