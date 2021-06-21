using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using NerdStore.Catalogo.Domain;
using NerdStore.Core.Data;
using NerdStore.Core.Messages;

namespace NerdStore.Catalogo.Data
{
    public class CatalogoContext : DbContextBase
    {
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        public CatalogoContext(DbContextOptions<CatalogoContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
            
            base.OnModelCreating(modelBuilder);
        }

    }

    public class ApplicationContextFactory : IDesignTimeDbContextFactory<CatalogoContext>
    {
        public CatalogoContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CatalogoContext>();
            builder
                .UseSqlServer("Server=localhost, 1433;" +
                              "Database=DominiosRicosDb;" +
                              "User Id=sa;" +
                              "Password=!123Senha;" +
                              "Application Name=\"DominiosRicos\";pooling=true;")
                .LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging();

            return new CatalogoContext(builder.Options);
        }
    }
}