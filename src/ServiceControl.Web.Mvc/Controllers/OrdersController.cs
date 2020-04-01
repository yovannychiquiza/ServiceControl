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

          
            var orderStateList =  _lookupAppService.GetOrderStateComboboxItems().Result;

            var orderStateSelectListItems = (from res in orderStateList.Items select new SelectListItem()
            {
                Text = res.DisplayText, Value = res.Value, Selected = res.Value == order.OrderStateId.ToString()
            }).ToList();


            var model = new EditOrderModalViewModel
            {
                Order = order,
                OrderState = orderStateSelectListItems
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

            var model = new EditOrderModalViewModel
            {
                Order = new OrderDto(),
                OrderState = orderStateSelectListItems
            };
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}
