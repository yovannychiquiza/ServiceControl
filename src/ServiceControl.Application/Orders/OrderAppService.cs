using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using ServiceControl.Orders.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Orders
{
    public enum OrderStateEnum
    {
        Booked = 1,
        Cancelled = 2,
        Delayed = 3,
        To_be_follow = 4
    }
    public class OrderAppService : ApplicationService, IOrderAppService
    {
        private readonly IRepository<Orders> _orderRepository;
        private readonly IAbpSession _session;
        public OrderAppService(IRepository<Orders> repository, IAbpSession session)
        {
            _orderRepository = repository;
            _session = session;
        }


        public async Task<PagedOrderResultResponseDto> GetAll(PagedOrderResultRequestDto input)
        {
            var query = _orderRepository.GetAll();
                
            if (!input.Keyword.IsNullOrWhiteSpace())
                query = query.Where(x => x.Serial.Contains(input.Keyword) || x.Company.Name.Contains(input.Keyword) || x.OrderState.Name.Contains(input.Keyword));
            if (input.DateFrom.HasValue)
                query = query.Where(x => x.DateBooked >= input.DateFrom);
            if (input.DateTo.HasValue)
                query = query.Where(x => x.DateBooked <= input.DateTo);

            var ordersList = query
                .Include(t => t.OrderState)
                .Include(t => t.Company)
                .Include(t => t.SalesRep)
                .Include(t => t.TimeSlot)
                .Include(t => t.FirstIdentification)
                .Include(t => t.SecondIdentification)
                .OrderByDescending(t => t.Id)
                .ToList();

            int count = ordersList.Count();
            var newList = ordersList.Skip(input.SkipCount).Take(input.MaxResultCount);

            ListResultDto<OrderListDto> ss = new ListResultDto<OrderListDto>();

            PagedOrderResultResponseDto pagedOrderResultResponseDto = new PagedOrderResultResponseDto();
            pagedOrderResultResponseDto.TotalCount = count;
            pagedOrderResultResponseDto.Data = new ListResultDto<OrderListDto>(
                ObjectMapper.Map<List<OrderListDto>>(newList));

            return pagedOrderResultResponseDto;
        }

        public async Task Create(OrderDto input)
        {
            try
            {
                input.SalesRepId = _session.UserId.GetValueOrDefault();
                input.OrderStateId = (int)OrderStateEnum.Booked;
                input.DateBooked = DateTime.Now;
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
