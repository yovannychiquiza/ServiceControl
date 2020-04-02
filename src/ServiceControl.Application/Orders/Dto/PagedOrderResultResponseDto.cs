using Abp.Application.Services.Dto;
using System;

namespace ServiceControl.Orders.Dto
{
    public class PagedOrderResultResponseDto : PagedResultRequestDto
    {
        public int TotalCount { get; set; }
        public ListResultDto<OrderListDto> Data { get; set; }
    }
}
