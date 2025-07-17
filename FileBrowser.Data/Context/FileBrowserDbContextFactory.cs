using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FileBrowser.Data.Context
{
    public class FileBrowserDbContextFactory : IDesignTimeDbContextFactory<FileBrowserDbContext>
    {
        public FileBrowserDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "FileBrowser.Api");
            var optionsBuilder = new DbContextOptionsBuilder<FileBrowserDbContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("FileBrowserConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new FileBrowserDbContext(optionsBuilder.Options);
        }
    }
}