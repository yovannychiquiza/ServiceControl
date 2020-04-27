using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ServiceControl.Authorization.Users;
using ServiceControl.Orders;
using System.Collections.Generic;

namespace ServiceControl.Orders.Dto
{
    [AutoMapTo(typeof(OrdersProductType))]
    public class OrdersProductTypeDto
    {
        public int OrdersId { get; set; }
        public int ProductTypeId { get; set; }
    }
}
