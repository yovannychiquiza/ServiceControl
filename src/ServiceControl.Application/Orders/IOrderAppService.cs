﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Http;
using ServiceControl.Orders.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceControl.Orders
{
    public interface IOrderAppService : IApplicationService
    {
        Task<PagedOrderResultResponseDto> GetAll(PagedOrderResultRequestDto input);
        Task Create(OrderDto input);
        Task GetOrderDelete(long id);
        Task<OrderDto> GetOrder(long id);
        Task<ExportResultResponse> GetExportExcel(PagedOrderResultRequestDto input);
        Task<ListResultDto<ComboboxItemDto>> GetCompanyComboboxItems(long id);
        Task GetOrderBooking(OrderDto input);
        Task GetBookingUpdate(OrderDto input);
        public List<OrderListDto> ReadInvoiceFile(IFormFile formFile);

    }

}
