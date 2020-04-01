using Abp.Application.Services.Dto;
using System;

namespace ServiceControl.Orders.Dto
{
    public class PagedOrderResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}
