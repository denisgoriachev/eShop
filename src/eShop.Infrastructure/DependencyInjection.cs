using eShop.Application.Persistance;
using eShop.Application.Service;
using eShop.Common;
using eShop.Domain;
using eShop.Infrastructure.Persistance;
using eShop.Infrastructure.Projections;
using eShop.Infrastructure.Services.Application;
using eShop.Infrastructure.Services.Domain;
using EventStore.Client;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eShop.Infrastructure
{
    public static class DependencyInjection
    {
        static Regex _versionRegex = new Regex(@"V\d*");

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterEventTypeNameMappings();

            services.AddSingleton(
                ctx =>
                    ConfigureEventStore(
                        configuration["EventStore:ConnectionString"],
                        ctx.GetService<ILoggerFactory>()!
                    )
            );

            services.AddSingleton<IAggregateStore, AggregateEventStore>();
            services.AddTransient<IDateTimeService, DateTimeService>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("eShopProjectionDatabase"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<ICheckpointProvider, DatabaseCheckpointProvider>();

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddHostedService(ctx =>
            {
                return new DatabaseProjectionService(
                    ctx.GetRequiredService<EventStoreClient>(),
                    "eShopProjectionDatabase",
                    ctx
                    );
            });

            return services;
        }

        private static void RegisterEventTypeNameMappings()
        {
            var domainEventTypes = typeof(IDomainEvent).Assembly.GetTypesThatImplementsInterface<IDomainEvent>();

            foreach (var domainEventType in domainEventTypes)
            {
                var parentClassName = domainEventType?.DeclaringType?.Name ?? throw new Exception($"Declaring type is not defined for type {domainEventType?.FullName}");

                if (!_versionRegex.IsMatch(parentClassName))
                    parentClassName = "";

                var suffix = string.IsNullOrWhiteSpace(parentClassName) ? "" : $"-{parentClassName}";

                EventTypeMap.AddType(domainEventType, $"{domainEventType.Name}{suffix}");
            }
        }

        private static EventStoreClient ConfigureEventStore(string connectionString, ILoggerFactory loggerFactory)
        {
            var settings = EventStoreClientSettings.Create(connectionString);
            settings.ConnectionName = "eshopApp";
            settings.LoggerFactory = loggerFactory;
            return new EventStoreClient(settings);
        }
    }
}
