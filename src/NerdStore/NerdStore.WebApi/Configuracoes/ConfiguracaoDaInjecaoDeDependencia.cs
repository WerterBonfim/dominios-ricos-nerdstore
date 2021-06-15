using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Data;
using NerdStore.Catalogo.Data.Repository;
using NerdStore.Catalogo.Domain;
using NerdStore.Catalogo.Domain.Events;
using NerdStore.Core.Bus;
using NerdStore.Vendas.Application.Commands;

namespace NerdStore.WebApi.Configuracoes
{
    public static class ConfiguracaoDaInjecaoDeDependencia
    {
        public static void RegistrarServicos(
            this IServiceCollection servicos,
            IConfiguration configuration
            )
        {
            // Domain Bus (Mediator)
            servicos.AddScoped<IMediatrHandler, MediatrHandler>();

            // Catalogo
            servicos.AddScoped<IProdutoRepository, ProdutoRepository>();
            servicos.AddScoped<IProdutoAppService, ProdutoService>();
            servicos.AddScoped<IEstoqueService, EstoqueService>();

            // Eventos de dominio
            servicos.AddScoped<INotificationHandler<ProdutoComEstoqueInferiorEvent>, ProdutoEventHandler>();
            
            // Vendas
            servicos.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();


            servicos.AddDbContext<CatalogoContext>(options =>
                options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information)
            );
            
            
            
        }
    }
}