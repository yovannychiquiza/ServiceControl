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
                //).AddItem(
                //    new MenuItemDefinition(
                //        PageNames.Tenants,
                //        L("Tenants"),
                //        url: "Tenants",
                //        icon: "fas fa-building",
                //        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants)
                //    )
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
                //.AddItem(
                //    new MenuItemDefinition(
                //        PageNames.About,
                //        L("About"),
                //        url: "About",
                //        icon: "fas fa-info-circle"
                //    )
                //)
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
                                url: "",
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
                        //).AddItem(
                        //    new MenuItemDefinition(
                        //        "AspNetBoilerplateSamples",
                        //        new FixedLocalizableString("Samples"),
                        //        url: "https://aspnetboilerplate.com/Samples?ref=abptmpl",
                        //        icon: "far fa-dot-circle"
                        //    )
                        //).AddItem(
                        //    new MenuItemDefinition(
                        //        "AspNetBoilerplateDocuments",
                        //        new FixedLocalizableString("Documents"),
                        //        url: "https://aspnetboilerplate.com/Pages/Documents?ref=abptmpl",
                        //        icon: "far fa-dot-circle"
                        //    )
                        //)
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
                        //).AddItem(
                        //    new MenuItemDefinition(
                        //        "AspNetZeroFaq",
                        //        new FixedLocalizableString("Faq"),
                        //        url: "https://aspnetzero.com/Faq?ref=abptmpl",
                        //        icon: "far fa-dot-circle"
                        //    )
                        //).AddItem(
                        //    new MenuItemDefinition(
                        //        "AspNetZeroDocuments",
                        //        new FixedLocalizableString("Documents"),
                        //        url: "https://aspnetzero.com/Documents?ref=abptmpl",
                        //        icon: "far fa-dot-circle"
                        //    )
                        //)
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ServiceControlConsts.LocalizationSourceName);
        }
    }
}
