using InvoiceApp.Api.Services.InvoiceService;
using InvoiceApp.Infrastructure.Abstracts;
using InvoiceApp.Infrastructure.GenericRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceApp.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IinvoiceService,InvoiceService>();

            return services;
        }

    }
}
