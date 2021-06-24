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
using NerdStore.Core.Communication;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.Notifications;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Application.Commands.Pedido;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Application.Queries;
using NerdStore.Vendas.Data;
using NerdStore.Vendas.Data.Repository;
using NerdStore.Vendas.Domain;

namespace NerdStore.WebApi.Configuracoes
{
    public static class ConfiguracaoDaInjecaoDeDependencia
    {
        public static void RegistrarServicos(
            this IServiceCollection servicos,
            IConfiguration configuration
            )
        {
            
            // Catalogo
            servicos.AddScoped<IProdutoRepository, ProdutoRepository>();
            servicos.AddScoped<IProdutoAppService, ProdutoService>();
            servicos.AddScoped<IEstoqueService, EstoqueService>();
            servicos.AddDbContext<CatalogoContext>(options =>
                options
                    .UseSqlServer(configuration.GetConnectionString("Catalogo"))
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information)
            );

            
            
            // Eventos de dominio
            servicos.AddScoped<INotificationHandler<ProdutoComEstoqueInferiorEvent>, ProdutoEventHandler>();
            
            
            
            // Vendas
            servicos.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
            servicos.AddScoped<IRequestHandler<AtualizarItemPedidoCommand, bool>, PedidoCommandHandler>();
            servicos.AddScoped<IRequestHandler<RemoverItemPedidoCommand, bool>, PedidoCommandHandler>();
            servicos.AddScoped<IRequestHandler<AplicarVoucherPedidoCommand, bool>, PedidoCommandHandler>();
            
            servicos.AddScoped<IPedidoRepository, PedidoRepository>();
            servicos.AddScoped<IPedidoQueries, PedidoQueries>();
            servicos.AddDbContext<VendasContext>(options =>
                options
                    .UseSqlServer(configuration.GetConnectionString("Vendas"))
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Information)
            );


            // Mediator
            servicos.AddScoped<IMediatrHandler, MediatrHandler>();
            
            // Notifications
            servicos.AddScoped<INotificationHandler<DomainNotificaton>, DomainNotificationHandler>();
            servicos.AddScoped<INotificationHandler<PedidoRascunhoIniciadoEvent>, PedidoEventHandler>();
            servicos.AddScoped<INotificationHandler<ItemDoPedidoAdicionadoEvent>, PedidoEventHandler>();
            servicos.AddScoped<INotificationHandler<PedidoAtualizadoEvent>, PedidoEventHandler>();



        }
    }
}