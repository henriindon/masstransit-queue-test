using MassTransit;
using MassTransitWebAppPubSub.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransitPubSubSender
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMassTransitBus(this IServiceCollection serviceCollection, AzureServiceBusConfiguration config)
        {
            serviceCollection.AddMassTransit(configurator =>
                {
                    configurator.UsingAzureServiceBus((context, cfg) =>
                    {
                        cfg.Host(config.ConnectionString);
                    });
                });

            return serviceCollection;
        }
    }
}
