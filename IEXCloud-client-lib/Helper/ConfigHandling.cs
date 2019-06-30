using System;
using Microsoft.Extensions.Configuration;
using static IEXCloudClient.NetCore.IEXCloudClientOptions;

namespace IEXCloudClient.NetCore.Helper
{
    public class ConfigHandling {
        public static IEXCloudClientOptions LoadConfig(string folder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(folder)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: false);
            var configuration = builder.Build();
            return ReadConfig(configuration);
        }

        public static IEXCloudClientOptions ReadConfig(IConfigurationRoot configuration)
        {
            var publicToken = configuration["PublishableToken"];
            var secretToken = configuration["SecretToken"];
            Enum.TryParse(configuration["Environment"], true, out IEXCloudClientOptionsEnvironment environment);
            Enum.TryParse(configuration["Version"], true, out IEXCloudClientOptionsVersion version);
            var options = new IEXCloudClientOptions(environment, version, publicToken, secretToken);
            return options;
        }
    }
}