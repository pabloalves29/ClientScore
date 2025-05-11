using GlobalAccount.Application.Interfaces;
using GlobalAccount.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalAccount.Application.Configuration
{
    public static class ApplicationConfiguration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Serviços da camada de aplicação
            services.AddScoped<IClientService, ClientService>();

            return services;
        }
    }
}
