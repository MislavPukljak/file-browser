using FileBrowser.Business.Mappings;
using FileBrowser.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FileBrowser.Business.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFolderService, FolderService>();

            return services;
        }
    }
}
