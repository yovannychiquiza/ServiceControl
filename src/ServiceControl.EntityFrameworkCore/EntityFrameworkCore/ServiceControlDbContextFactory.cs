using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ServiceControl.Configuration;
using ServiceControl.Web;

namespace ServiceControl.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class ServiceControlDbContextFactory : IDesignTimeDbContextFactory<ServiceControlDbContext>
    {
        public ServiceControlDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ServiceControlDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            ServiceControlDbContextConfigurer.Configure(builder, configuration.GetConnectionString(ServiceControlConsts.ConnectionStringName));

            return new ServiceControlDbContext(builder.Options);
        }
    }
}
