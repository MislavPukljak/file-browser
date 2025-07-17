using FileBrowser.Business.Extensions;
using FileBrowser.Data.Extensions;

namespace FileBrowser.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataServices(configuration);
            services.AddBusinessServices();

            return services;
        }
    }
}
