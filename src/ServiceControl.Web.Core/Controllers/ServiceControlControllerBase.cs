using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace ServiceControl.Controllers
{
    public abstract class ServiceControlControllerBase: AbpController
    {
        protected ServiceControlControllerBase()
        {
            LocalizationSourceName = ServiceControlConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
