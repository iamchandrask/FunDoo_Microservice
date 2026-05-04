using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration.Json;

namespace Fundoo.LabelService.Infrastructure.Persistence
{
    public class LabelDbContextFactory : IDesignTimeDbContextFactory<LabelDbContext>
    {
        public LabelDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Fundoo.LabelService.API"))
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<LabelDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new LabelDbContext(optionsBuilder.Options);
        }
    }
}