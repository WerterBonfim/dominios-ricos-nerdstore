using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace NerdStore.WebApi.Configuracoes
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AdicionarConfiguracaoDoSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NerdStore Domínios Ricos API",
                    Description = "Esta API foi desenvolvida baseada em varios cursos do site desenvolvedor.io",
                    Contact = new OpenApiContact {Name = "Werter Bonfim", Email = "werter@hotmail.com.br"},
                    License = new OpenApiLicense {Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT")}
                });
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                
                
            });

            return services;
        }

        public static IApplicationBuilder UseConfiguracoesDoSwagger(this IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
            }

            return app;
        }
    }
}