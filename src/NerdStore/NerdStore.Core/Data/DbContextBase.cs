using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace NerdStore.Core.Data
{
    public abstract class DbContextBase : DbContext, IUnitOfWork
    {

        public DbContextBase(DbContextOptions options) : base(options)
        {
            
        }
        
        public async Task<bool> Commit()
        {
            var camposDataCadastro = ChangeTracker
                .Entries()
                .Where(e => e.Entity.GetType().GetProperty("DataCadastro") != null);
            foreach (var entry in camposDataCadastro)
            {
                if (entry.State == EntityState.Added)
                    entry.Property("DataCadastro").CurrentValue = DateTime.UtcNow;

                if (entry.State == EntityState.Modified)
                    entry.Property("DataCadastro").IsModified = false;
            }

            return await base.SaveChangesAsync() > 0;
        }
        
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            
            ForcarVarchar(modelBuilder);

            // var relacoes = modelBuilder.Model
            //     .GetEntityTypes()
            //     .SelectMany(x => x.GetForeignKeys());
            //
            // foreach (var relacao in relacoes)
            //     relacao.DeleteBehavior = DeleteBehavior.SetNull;

            base.OnModelCreating(modelBuilder);
        }
        
        private static void ForcarVarchar(ModelBuilder modelBuilder)
        {
            var camposNVarchar = modelBuilder.Model.GetEntityTypes()
                .SelectMany(x => x.GetProperties())
                .Where(x => x.ClrType == typeof(string) && x.GetColumnType() == null);
            foreach (var property in camposNVarchar)
                property.SetColumnType("varchar(100)");
        }
    }
}