using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ServiceControl.Authorization.Users;
using ServiceControl.Orders;
using System.Collections.Generic;

namespace ServiceControl.UserCompany.Dto
{
    [AutoMapTo(typeof(SalesRepCompany))]
    public class CompanyReponseDto : EntityDto<long>
    {
        public List<VendorCompanyDto> CompanyList { get; set; }
    }

    public class VendorCompanyDto 
    {
        public string Id { get; set; }
        public string Code { get; set; }
    }
}
