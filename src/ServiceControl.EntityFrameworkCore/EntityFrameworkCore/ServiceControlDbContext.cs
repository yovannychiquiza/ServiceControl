using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ServiceControl.Authorization.Roles;
using ServiceControl.Authorization.Users;
using ServiceControl.MultiTenancy;
using ServiceControl.UserCompany;
using ServiceControl.Orders;

namespace ServiceControl.EntityFrameworkCore
{
    public class ServiceControlDbContext : AbpZeroDbContext<Tenant, Role, User, ServiceControlDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<ServiceControl.Orders.Orders> Orders { get; set; }
        public DbSet<ServiceControl.Orders.OrderState> OrderState { get; set; }
        public DbSet<ServiceControl.Orders.Company> Company { get; set; }
        public DbSet<ServiceControl.Orders.FirstIdentification> FirstIdentification { get; set; }
        public DbSet<ServiceControl.Orders.SecondIdentification> SecondIdentification { get; set; }
        public DbSet<ServiceControl.Orders.TimeSlot> TimeSlot { get; set; }
        public DbSet<SalesRepCompany> SalesRepCompany { get; set; }
        public DbSet<SalesRepSerial> SalesRepSerial { get; set; }
        public DbSet<SubSalesRep> SubSalesRep { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<OrdersProductType> OrdersProductType { get; set; }
        
        public ServiceControlDbContext(DbContextOptions<ServiceControlDbContext> options)
            : base(options)
        {
        }
    }
}
