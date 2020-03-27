﻿using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ServiceControl.Authorization.Roles;
using ServiceControl.Authorization.Users;
using ServiceControl.MultiTenancy;

namespace ServiceControl.EntityFrameworkCore
{
    public class ServiceControlDbContext : AbpZeroDbContext<Tenant, Role, User, ServiceControlDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public ServiceControlDbContext(DbContextOptions<ServiceControlDbContext> options)
            : base(options)
        {
        }
    }
}
