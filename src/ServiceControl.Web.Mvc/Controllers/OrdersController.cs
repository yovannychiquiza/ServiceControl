using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using ServiceControl.Authorization;
using ServiceControl.Controllers;
using ServiceControl.Orders;
using ServiceControl.Web.Models.Orders;
using ServiceControl.Orders.Dto;
using ServiceControl.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Collections.Generic;

namespace ServiceControl.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Users)]
    public class OrdersController : ServiceControlControllerBase
    {
        private readonly IOrderAppService _orderAppService;
        private readonly ILookupAppService _lookupAppService;

        public OrdersController(IOrderAppService orderAppService, ILookupAppService lookupAppService)
        {
            _orderAppService = orderAppService;
            _lookupAppService = lookupAppService;
        }

        public async Task<ActionResult> EditModal(long orderId)
        {
            var order = await _orderAppService.GetOrder(orderId);

          
            var orderStateList = _lookupAppService.GetOrderStateComboboxItems().Result;
            var companyList = _lookupAppService.GetCompanyComboboxItems().Result;

            var orderStateSelectListItems = (from res in orderStateList.Items select new SelectListItem()
            {
                Text = res.DisplayText, Value = res.Value, Selected = res.Value == order.OrderStateId.ToString()
            }).ToList();
            var companySelectListItems = (from res in companyList.Items
                                             select new SelectListItem()
                                             {
                                                 Text = res.DisplayText,
                                                 Value = res.Value,
                                                 Selected = res.Value == order.CompanyId.ToString()
                                             }).ToList();


            var existingAccountNoList = _lookupAppService.GetExistingAccountNoItems();
            var existingAccountNoItems = (from res in existingAccountNoList.Items
                                             select new SelectListItem()
                                             {
                                                 Text = res.DisplayText,
                                                 Value = res.Value,
                                                 Selected = res.Value == order.ExistingAccountNo.ToString()
                                             }).ToList();


            var identificationList = _lookupAppService.GetIdentificationItems();
            var identificationItems = (from res in identificationList.Items
                                          select new SelectListItem()
                                          {
                                              Text = res.DisplayText,
                                              Value = res.Value,
                                              Selected = res.Value == order.PrimaryId.ToString()
                                          }).ToList();

            var model = new EditOrderModalViewModel
            {
                Order = order,
                OrderState = orderStateSelectListItems,
                Company = companySelectListItems,
                ExistingAccountNo = existingAccountNoItems,
                Identification = identificationItems
            };

            return PartialView("_EditModal", model);
        }

        public async Task<ActionResult> Index()
        {
            var orderStateList = _lookupAppService.GetOrderStateComboboxItems().Result;
            var companyList = _lookupAppService.GetCompanyComboboxItems().Result;


            var orderStateSelectListItems = (from res in orderStateList.Items
                                             select new SelectListItem()
                                             {
                                                 Text = res.DisplayText,
                                                 Value = res.Value,
                                             }).ToList();
            var companySelectListItems = (from res in companyList.Items
                                          select new SelectListItem()
                                          {
                                              Text = res.DisplayText,
                                              Value = res.Value,
                                          }).ToList();

            var existingAccountNoList = _lookupAppService.GetExistingAccountNoItems();
            var existingAccountNoItems = (from res in existingAccountNoList.Items
                                          select new SelectListItem()
                                          {
                                              Text = res.DisplayText,
                                              Value = res.Value,
                                          }).ToList();

            var identificationList = _lookupAppService.GetIdentificationItems();
            var identificationItems = (from res in identificationList.Items
                                       select new SelectListItem()
                                       {
                                           Text = res.DisplayText,
                                           Value = res.Value,
                                       }).ToList();

            var model = new EditOrderModalViewModel
            {
                Order = new OrderDto(),
                OrderState = orderStateSelectListItems,
                Company = companySelectListItems,
                ExistingAccountNo = existingAccountNoItems,
                Identification = identificationItems

            };
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}
