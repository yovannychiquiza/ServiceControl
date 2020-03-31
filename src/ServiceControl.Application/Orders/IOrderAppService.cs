﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ServiceControl.Orders.Dto;
using System.Threading.Tasks;

namespace ServiceControl.Orders
{
    public interface IOrderAppService : IApplicationService
    {
        Task<ListResultDto<OrderListDto>> GetAll();
        Task Create(OrderDto input);
        Task GetOrderDelete(long id);
        Task<OrderDto> GetOrder(long id);
    }

}