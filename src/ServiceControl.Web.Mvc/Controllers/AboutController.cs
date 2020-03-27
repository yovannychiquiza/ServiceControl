using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using ServiceControl.Controllers;

namespace ServiceControl.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : ServiceControlControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
