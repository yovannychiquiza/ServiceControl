using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace ServiceControl.Orders.Dto
{
    public class PagedOrderResultResponseDto : PagedResultRequestDto
    {
        public int TotalCount { get; set; }
        public List<ProductTypeDto> ProductType { get; set; }
        public ListResultDto<OrderListDto> Data { get; set; }
    }
}
