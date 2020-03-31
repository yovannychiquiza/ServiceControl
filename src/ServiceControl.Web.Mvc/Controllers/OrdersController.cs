using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using ServiceControl.Authorization;
using ServiceControl.Controllers;
using ServiceControl.Orders;
using ServiceControl.Web.Models.Orders;
using ServiceControl.Orders.Dto;

namespace ServiceControl.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Users)]
    public class OrdersController : ServiceControlControllerBase
    {
        private readonly IOrderAppService _orderAppService;

        public OrdersController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        public async Task<ActionResult> EditModal(long orderId)
        {
            var order = await _orderAppService.GetOrder(orderId);
            var model = new EditOrderModalViewModel
            {
                Order = order,
            };
            return PartialView("_EditModal", model);
        }

        public async Task<ActionResult> Index()
        {
            var model = new EditOrderModalViewModel
            {
                Order = new OrderDto()
            };
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}
