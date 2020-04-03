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
            var orderStateSelectListItems = (from res in orderStateList.Items
                                             select new SelectListItem()
                                             {
                                                 Text = res.DisplayText,
                                                 Value = res.Value,
                                                 Selected = res.Value == order.OrderStateId.ToString()
                                             }).ToList();

            var companyList = _lookupAppService.GetCompanyComboboxItems().Result;
            var companySelectListItems = (from res in companyList.Items
                                             select new SelectListItem()
                                             {
                                                 Text = res.DisplayText,
                                                 Value = res.Value,
                                                 Selected = res.Value == order.CompanyId.ToString()
                                             }).ToList();

            var firstIdentificationList = _lookupAppService.GetFirstIdentificationItems().Result;
            var firstIdentificationSelectListItems = (from res in firstIdentificationList.Items
                                          select new SelectListItem()
                                          {
                                              Text = res.DisplayText,
                                              Value = res.Value,
                                              Selected = res.Value == order.FirstIdentificationId.ToString()
                                          }).ToList();

            var secondIdentificationList = _lookupAppService.GetSecondIdentificationItems().Result;
            var secondIdentificationSelectListItems = (from res in secondIdentificationList.Items
                                                 select new SelectListItem()
                                                 {
                                                     Text = res.DisplayText,
                                                     Value = res.Value,
                                                     Selected = res.Value == order.SecondIdentificationId.ToString()
                                                 }).ToList();

            var timeSlotList = _lookupAppService.GetTimeSlotComboboxItems().Result;
            var timeSlotSelectListItems = (from res in timeSlotList.Items
                                             select new SelectListItem()
                                             {
                                                 Text = res.DisplayText,
                                                 Value = res.Value,
                                                 Selected = res.Value == order.TimeSlotId.ToString()
                                             }).ToList();
           

            var model = new EditOrderModalViewModel
            {
                Order = order,
                OrderState = orderStateSelectListItems,
                Company = companySelectListItems,
                FirstIdentification = firstIdentificationSelectListItems,
                SecondIdentification = secondIdentificationSelectListItems,
                TimeSlot = timeSlotSelectListItems
            };

            return PartialView("_EditModal", model);
        }

        public async Task<ActionResult> Index()
        {
            var orderStateList = _lookupAppService.GetOrderStateComboboxItems().Result;
            var orderStateSelectListItems = (from res in orderStateList.Items
                                             select new SelectListItem()
                                             {
                                                 Text = res.DisplayText,
                                                 Value = res.Value,
                                             }).ToList();

            var companyList = _lookupAppService.GetCompanyComboboxItems().Result;
            var companySelectListItems = (from res in companyList.Items
                                          select new SelectListItem()
                                          {
                                              Text = res.DisplayText,
                                              Value = res.Value,
                                          }).ToList();

            var firstIdentificationList = _lookupAppService.GetFirstIdentificationItems().Result;
            var firstIdentificationSelectListItems = (from res in firstIdentificationList.Items
                                                      select new SelectListItem()
                                                      {
                                                          Text = res.DisplayText,
                                                          Value = res.Value,
                                                      }).ToList();

            var secondIdentificationList = _lookupAppService.GetSecondIdentificationItems().Result;
            var secondIdentificationSelectListItems = (from res in secondIdentificationList.Items
                                                       select new SelectListItem()
                                                       {
                                                           Text = res.DisplayText,
                                                           Value = res.Value,
                                                       }).ToList();

            var timeSlotList = _lookupAppService.GetTimeSlotComboboxItems().Result;
            var timeSlotSelectListItems = (from res in timeSlotList.Items
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
                FirstIdentification = firstIdentificationSelectListItems,
                SecondIdentification = secondIdentificationSelectListItems,
                TimeSlot = timeSlotSelectListItems
            };
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}
