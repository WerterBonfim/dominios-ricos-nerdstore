using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using NerdStore.Catalogo.Domain;
using NerdStore.Core.Data;

namespace NerdStore.Catalogo.Data
{
    public class CatalogoContext : DbContext, IUnitOfWork
    {
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        public CatalogoContext(DbContextOptions<CatalogoContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoContext).Assembly);
            ForcarVarchar(modelBuilder);
        }

        private static void ForcarVarchar(ModelBuilder modelBuilder)
        {
            var camposNVarchar = modelBuilder.Model.GetEntityTypes()
                .SelectMany(x => x.GetProperties())
                .Where(x => x.ClrType == typeof(string) && x.GetColumnType() == null);
            foreach (var property in camposNVarchar)
                property.SetColumnType("varchar(100)");
        }

        public async Task<bool> Commit()
        {
            return await SaveChangesAsync() > 0;
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