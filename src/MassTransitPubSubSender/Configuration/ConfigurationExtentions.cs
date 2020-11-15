using Microsoft.Extensions.Configuration;

namespace MassTransitWebAppPubSub.Configuration
{
    public static class ConfigurationExtentions
    {
        public static AzureServiceBusConfiguration GetAzureServiceBusConfig(this IConfiguration configuration)
            => configuration.GetSection(nameof(AzureServiceBusConfiguration)).Get<AzureServiceBusConfiguration>();
    }
}
