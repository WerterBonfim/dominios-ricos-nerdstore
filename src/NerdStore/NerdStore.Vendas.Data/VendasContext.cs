using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NerdStore.Core.Data;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Data
{
    public class VendasContext : DbContextBase
    {
        public VendasContext(DbContextOptions<VendasContext> options) : base(options)
        {
        }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItens { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();
            
            modelBuilder.HasSequence<int>("MinhaSequencia")
                .StartsAt(1000)
                .IncrementsBy(1);
            
            base.OnModelCreating(modelBuilder);
        }
    }
    
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<VendasContext>
    {
        public VendasContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<VendasContext>();
            builder
                .UseSqlServer("Server=localhost, 1433;" +
                              "Database=DominiosRicosDb;" +
                              "User Id=sa;" +
                              "Password=!123Senha;" +
                              "Application Name=\"DominiosRicos\";pooling=true;")
                .LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging();

            return new VendasContext(builder.Options);
        }
    }
}