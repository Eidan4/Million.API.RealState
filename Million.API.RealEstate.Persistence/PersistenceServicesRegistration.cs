using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Million.API.RealEstate.Persistence.DBContext;
using Microsoft.Extensions.Configuration;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Persistence.Repositories.CrossRepositories;
using Million.API.RealEstate.Persistence.Repositories;


namespace Million.API.RealEstate.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Registrar IMongoClient
            services.AddSingleton<IMongoClient>(sp =>
            {
                var connectionString = configuration.GetConnectionString("MongoDb");
                return new MongoClient(connectionString);
            });

            // Registrar IMongoDatabase
            services.AddSingleton<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var databaseName = configuration.GetSection("DatabaseSettings:DatabaseName").Value;
                return client.GetDatabase(databaseName);
            });

            // Registrar UnitOfWork
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
