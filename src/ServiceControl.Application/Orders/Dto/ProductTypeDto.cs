using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ServiceControl.Authorization.Users;
using ServiceControl.Orders;
using System.Collections.Generic;

namespace ServiceControl.Orders.Dto
{
    [AutoMapTo(typeof(ProductType))]
    public class ProductTypeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
