﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
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


        public async Task<ListResultDto<OrderListDto>> GetAll()
        {
            var tasks = await _orderRepository
                .GetAll()
                .OrderByDescending(t => t.Company)
                .ToListAsync();

            return new ListResultDto<OrderListDto>(
                ObjectMapper.Map<List<OrderListDto>>(tasks)
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