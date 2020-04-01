﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using ServiceControl.Orders.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Orders
{
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly IRepository<Orders> _orderRepository;

        public OrderAppService(IRepository<Orders> repository)
        {
            _orderRepository = repository;
        }


        public async Task<ListResultDto<OrderListDto>> GetAll(PagedOrderResultRequestDto input)
        {
            var query = _orderRepository.GetAllIncluding(t => t.OrderState);
            if (!input.Keyword.IsNullOrWhiteSpace())
                query = query.Where(x => x.Serial.Contains(input.Keyword) || x.Company.Contains(input.Keyword) || x.OrderState.Name.Contains(input.Keyword));
            if (input.DateFrom.HasValue)
                query = query.Where(x => x.DateBooked >= input.DateFrom);
            if (input.DateTo.HasValue)
                query = query.Where(x => x.DateBooked <= input.DateTo);

            var ordersList = query.OrderByDescending(t => t.Id).ToList();

            return new ListResultDto<OrderListDto>(
                ObjectMapper.Map<List<OrderListDto>>(ordersList)
            );
        }

        public async Task Create(OrderDto input)
        {
            try
            {
                var task = ObjectMapper.Map<Orders>(input);
                await _orderRepository.InsertAsync(task);
            }
            catch (Exception e)
            {
                string mess = e.Message;
            }
        }
        public async Task GetOrderDelete(long id)
        {
            var model = _orderRepository.FirstOrDefault(t => t.Id == id);
            _orderRepository.DeleteAsync(model);
        }
        public async Task Update(OrderDto input)
        {
            var task = ObjectMapper.Map<Orders>(input);
            await _orderRepository.UpdateAsync(task);
        }

        public Task<OrderDto> GetOrder(long id)
        {
            Orders model = _orderRepository.FirstOrDefault(t => t.Id == id);
            OrderDto dto = new OrderDto(); 
            dto = ObjectMapper.Map(model, dto);
            return Task.FromResult(dto);        
        }
       
    }
}
