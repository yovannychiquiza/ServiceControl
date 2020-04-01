using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using ServiceControl.Authorization;

namespace ServiceControl.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class ServiceControlNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Home,
                        L("HomePage"),
                        url: "",
                        icon: "fas fa-home",
                        requiresAuthentication: true
                    )
                
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Users,
                        L("Users"),
                        url: "Users",
                        icon: "fas fa-users",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users)
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Roles,
                        L("Roles"),
                        url: "Roles",
                        icon: "fas fa-theater-masks",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
                            )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Orders,
                        L("Orders"),
                        url: "Orders",
                        icon: "fas fa-list-alt",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
                            )
                )
               
                .AddItem( // Menu items below is just for demonstration!
                    new MenuItemDefinition(
                        "MultiLevelMenu",
                        L("MultiLevelMenu"),
                        icon: "fas fa-circle"
                    ).AddItem(
                        new MenuItemDefinition(
                            "AspNetBoilerplate",
                            new FixedLocalizableString("Orders"),
                            icon: "far fa-circle"
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetBoilerplateHome",
                                new FixedLocalizableString("Booking status"),
                                url: "Orders",
                                icon: "far fa-dot-circle"
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetBoilerplateTemplates",
                                new FixedLocalizableString("Statistics"),
                                url: "",
                                icon: "far fa-dot-circle"
                            )
                        )
                       
                    ).AddItem(
                        new MenuItemDefinition(
                            "AspNetZero",
                            new FixedLocalizableString("Report"),
                            icon: "far fa-circle"
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetZeroHome",
                                new FixedLocalizableString("Invoice"),
                                url: "",
                                icon: "far fa-dot-circle"
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetZeroFeatures",
                                new FixedLocalizableString("Montly"),
                                url: "",
                                icon: "far fa-dot-circle"
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                "AspNetZeroPricing",
                                new FixedLocalizableString("Sales"),
                                url: "",
                                icon: "far fa-dot-circle"
                            )
                        )
                       
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ServiceControlConsts.LocalizationSourceName);
        }
    }
}
