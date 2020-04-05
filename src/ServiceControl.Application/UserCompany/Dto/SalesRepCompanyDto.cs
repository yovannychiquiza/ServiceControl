using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ServiceControl.Authorization.Users;
using ServiceControl.Orders;

namespace ServiceControl.UserCompany.Dto
{
    [AutoMapTo(typeof(SalesRepCompany))]
    public class SalesRepCompanyDto : EntityDto<long>
    {

        public Company Company { get; set; }
        public int CompanyId { get; set; }

        public long SalesRepId { get; set; }

        public User SalesRep { get; set; }

        public string Code { get; set; }

    }
}
