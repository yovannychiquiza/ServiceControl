using Abp.Application.Services.Dto;
using System;

namespace ServiceControl.Orders.Dto
{
    public class PagedOrderResultRequestDto : PagedResultRequestDto
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string CompanyId { get; set; }
        public string OrderStateId { get; set; }
        public string Followed { get; set; }
        public string Sgi { get; set; }
        public string InvoiceNo { get; set; }
        public int PaymentStatusId { get; set; }
    }
}
