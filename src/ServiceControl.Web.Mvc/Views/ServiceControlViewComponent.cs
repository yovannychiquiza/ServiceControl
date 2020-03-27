using Abp.AspNetCore.Mvc.ViewComponents;

namespace ServiceControl.Web.Views
{
    public abstract class ServiceControlViewComponent : AbpViewComponent
    {
        protected ServiceControlViewComponent()
        {
            LocalizationSourceName = ServiceControlConsts.LocalizationSourceName;
        }
    }
}
