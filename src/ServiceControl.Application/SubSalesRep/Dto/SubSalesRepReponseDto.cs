using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ServiceControl.Authorization.Users;
using ServiceControl.Orders;
using System.Collections.Generic;

namespace ServiceControl.SubUser.Dto
{
    public class SubSalesRepReponseDto : EntityDto<long>
    {
        public List<SubSalesRepDto> SubSalesRepList { get; set; }
    }
}
