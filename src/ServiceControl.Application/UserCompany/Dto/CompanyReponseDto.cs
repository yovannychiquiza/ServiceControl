using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ServiceControl.Authorization.Users;
using ServiceControl.Orders;

namespace ServiceControl.UserCompany.Dto
{
    [AutoMapTo(typeof(SalesRepCompany))]
    public class CompanyReponseDto : EntityDto<long>
    {
        public string[] CompanyList { get; set; }
    }
}
