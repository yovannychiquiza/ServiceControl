using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ServiceControl.Authorization
{
    public class ServiceControlAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Pages_Orders, L("Orders"));
            context.CreatePermission(PermissionNames.Pages_Orders_Admin, L("OrdersAdmin"));
            context.CreatePermission(PermissionNames.Pages_Booking, L("Booking"));
            context.CreatePermission(PermissionNames.Pages_Booking_Admin, L("BookingAdmin"));
            context.CreatePermission(PermissionNames.Order_Ready, L("OrderReady"));
            context.CreatePermission(PermissionNames.Order_Admin_Ready, L("OrderAdminReady"));
            context.CreatePermission(PermissionNames.Order_See_All, L("OrderSeeAll"));
            context.CreatePermission(PermissionNames.Assign_Company, L("AssignCompany"));
            context.CreatePermission(PermissionNames.Order_Admin_Invoice, L("OrderAdminInvoice"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ServiceControlConsts.LocalizationSourceName);
        }
    }
}
