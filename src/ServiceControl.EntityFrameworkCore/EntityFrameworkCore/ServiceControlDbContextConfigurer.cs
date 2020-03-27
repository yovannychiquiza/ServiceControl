using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ServiceControl.EntityFrameworkCore
{
    public static class ServiceControlDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ServiceControlDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ServiceControlDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
