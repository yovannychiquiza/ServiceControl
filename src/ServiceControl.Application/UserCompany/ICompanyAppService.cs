using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ServiceControl.Orders;
using ServiceControl.UserCompany.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceControl.UserCompany
{
    public interface ICompanyAppService : IApplicationService
    {
        Task<List<SalesRepCompanyDto>> GetSalesRepCompany(long id);
        Task<List<Company>> GetCompany();
        Task Update(CompanyReponseDto input);
    }

}
