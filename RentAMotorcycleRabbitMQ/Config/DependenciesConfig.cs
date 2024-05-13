using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RentAMotorcycleRabbitMQ.Interface;
using RentAMotorcycleRabbitMQ.Models;
using RentAMotorcycleRabbitMQ.Repository;
using System;

namespace RentAMotorcycleRabbitMQ.Config
{
    public static class DependenciesConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<IStartProcess, StartProcess>();
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            Console.WriteLine("Environment: {0}", environmentName);

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            services.AddSingleton<IRabbitSettings>(configuration.GetSection("RabbitConfiguration").Get<RabbitSettings>());

            MongoClientSettings mongoCollection = MongoClientSettings.FromConnectionString(configuration.GetSection("MongoHost:RentMotors").Value);
            mongoCollection.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };

            services.AddScoped<IMongoClient>(c =>
            {
                return new MongoClient(mongoCollection);
            });

            return services;
        }
    }
}
