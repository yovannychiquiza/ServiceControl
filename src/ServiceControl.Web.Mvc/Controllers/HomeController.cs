using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using ServiceControl.Controllers;

namespace ServiceControl.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : ServiceControlControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
