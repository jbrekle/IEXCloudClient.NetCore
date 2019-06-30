using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace IEXCloudClient.NetCore.Helper
{
    public static class IEXCloudCliebtServiceCollectionExtensions
    {
        public static IServiceCollection AddIEXClientWithAppSettingsConfig(this IServiceCollection services) {
            var options = ConfigHandling.LoadConfig(Directory.GetCurrentDirectory());
            services.AddIEXClient(options);
            return services;
        }
        
        public static IServiceCollection AddIEXClient(this IServiceCollection services, IEXCloudClientOptions options) {
            services.AddHttpClient();
            services.AddSingleton<IIEXCloudClient, IEXCloudClient>();
            services.AddSingleton(options);
            return services;
        }
    }
}