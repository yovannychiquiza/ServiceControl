using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace ServiceControl.Web.Views
{
    public abstract class ServiceControlRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected ServiceControlRazorPage()
        {
            LocalizationSourceName = ServiceControlConsts.LocalizationSourceName;
        }
    }
}
