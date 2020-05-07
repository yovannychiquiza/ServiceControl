using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using ServiceControl.Authorization;
using ServiceControl.Controllers;
using ServiceControl.Users;
using ServiceControl.Web.Models.Users;
using ServiceControl.UserCompany;
using ServiceControl.SubUser;
using Abp.Runtime.Session;

namespace ServiceControl.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Users)]
    public class UsersController : ServiceControlControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly ICompanyAppService _salesRepCompanyAppService;
        private readonly ISubSalesRepAppService _subSalesRepAppService;
        private readonly IAbpSession _session;


        public UsersController(IUserAppService userAppService,
            ICompanyAppService salesRepCompanyAppService,
            ISubSalesRepAppService subSalesRepAppService,
            IAbpSession session
            )
        {
            _userAppService = userAppService;
            _salesRepCompanyAppService = salesRepCompanyAppService;
            _subSalesRepAppService = subSalesRepAppService;
            _session = session;
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
            var companyList = await _salesRepCompanyAppService.GetSalesRepCompany(_session.UserId.GetValueOrDefault());
            var salesRepCompanyDto = await _salesRepCompanyAppService.GetSalesRepCompany(userId);
            
            var model = new SalesRepCompanyModalViewModel
            {
                User = user,
                Company = companyList,
                SalesRepCompanyDto = salesRepCompanyDto
            };
            return PartialView("_CompanyModal", model);
        }

        public async Task<ActionResult> EditSubSalesRepModal(long userId)
        {
            var user = await _userAppService.GetAsync(new EntityDto<long>(userId));
            var userList = await _subSalesRepAppService.GetSalesRep();
            var subSalesRepDto = await _subSalesRepAppService.GetSubSalesRep(userId);

            var model = new SubSalesRepModalViewModel
            {
                User = user,
                SalesRep = userList,
                SubSalesRepDto = subSalesRepDto
            };
            return PartialView("_SubSalesRepModal", model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}
