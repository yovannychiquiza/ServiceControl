using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using ServiceControl.Authorization;
using ServiceControl.Controllers;
using ServiceControl.Users;
using ServiceControl.Web.Models.Users;
using ServiceControl.Common;
using ServiceControl.Orders;
using ServiceControl.UserCompany;

namespace ServiceControl.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Users)]
    public class UsersController : ServiceControlControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly ICompanyAppService _salesRepCompanyAppService;


        public UsersController(IUserAppService userAppService, ICompanyAppService salesRepCompanyAppService)
        {
            _userAppService = userAppService;
            _salesRepCompanyAppService = salesRepCompanyAppService;

        }

        public async Task<ActionResult> Index()
        {
            var roles = (await _userAppService.GetRoles()).Items;
            var model = new UserListViewModel
            {
                Roles = roles
            };
            return View(model);
        }

        public async Task<ActionResult> EditModal(long userId)
        {
            var user = await _userAppService.GetAsync(new EntityDto<long>(userId));
            var roles = (await _userAppService.GetRoles()).Items;
            var model = new EditUserModalViewModel
            {
                User = user,
                Roles = roles
            };
            return PartialView("_EditModal", model);
        }

        public async Task<ActionResult> EditCompanyModal(long userId)
        {
            var user = await _userAppService.GetAsync(new EntityDto<long>(userId));
            var companyList = await _salesRepCompanyAppService.GetCompany();
            var salesRepCompanyDto = await _salesRepCompanyAppService.GetSalesRepCompany(userId);
            
            var model = new SalesRepCompanyModalViewModel
            {
                User = user,
                Company = companyList,
                SalesRepCompanyDto = salesRepCompanyDto
            };
            return PartialView("_CompanyModal", model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}
