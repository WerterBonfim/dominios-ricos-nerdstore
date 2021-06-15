using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NerdStore.WebApi.Extencoes;

namespace NerdStore.WebApi.Configuracoes
{
    public static class IdentityConfig
    {
        public static IServiceCollection AdicionarConfiguracaoDeIdentidade(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            
            ConfiguracaoJwt(services, configuration);
            
            services.AddCors(options =>
                options.AddPolicy("Total", builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()));

            return services;
        }

        private static void ConfiguracaoJwt(IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = appSettings.ValidoEm,
                        ValidIssuer = appSettings.Emissor
                    };
                    
                    
                });
        }

        public static IApplicationBuilder UsarConfiguracaoDeIdentidade(this IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            app.UseCors("Total");
            
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}