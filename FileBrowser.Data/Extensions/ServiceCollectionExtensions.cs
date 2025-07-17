using FileBrowser.Data.Context;
using FileBrowser.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileBrowser.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FileBrowserDbContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("FileBrowserConnection")));

            services.AddScoped<IUnitOfWork,  UnitOfWork>();

            return services;
        }
    }
}
