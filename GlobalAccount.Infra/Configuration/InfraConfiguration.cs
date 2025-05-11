using GlobalAccount.Infra.Interfaces;
using GlobalAccount.Infra.Repositories;
using GlobalAccount.Infra.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAccount.Infra.Configuration
{
    public static class InfraConfiguration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Adiciona repositórios
            services.AddScoped<IClientRepository, ClientRepository>();

            // Adiciona a configuração como singleton para ser usada onde necessário
            services.AddSingleton(configuration);
            services.AddSingleton<TokenService>();

            return services;
        }
    }
}
