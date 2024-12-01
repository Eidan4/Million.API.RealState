using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Million.API.RealEstate.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Registrar AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Registrar Mediator (si usas MediatR para manejar comandos y queries)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Registrar servicios específicos de la capa Application
            // Ejemplo:
            // services.AddScoped<IMyApplicationService, MyApplicationService>();

            return services;
        }
    }
}