using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NerdStore.Catalogo.Application.AutoMapper;

namespace NerdStore.WebApi.Configuracoes
{
    public static class ApiConfig
    {
        public static IServiceCollection AdicionarConfiguracaoDaApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            
            services.AddAutoMapper(
                typeof(DominioParaViewModelMappingProfile),
                typeof(ViewModelParaDominioMappingProfile)
                );

            services.AddMediatR(typeof(Startup));

            services.AdicionarConfiguracaoDeIdentidade(configuration);
           
            
            return services;
        }

        public static IApplicationBuilder UseConfiguracoesDaApi(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UsarConfiguracaoDeIdentidade(env);

            app.UseEndpoints(endpoints => endpoints.MapControllers());
            return app;
        }
    }
}