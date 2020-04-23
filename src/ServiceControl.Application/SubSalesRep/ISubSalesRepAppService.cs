using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ServiceControl.Authorization.Users;
using ServiceControl.Orders;
using ServiceControl.SubUser.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceControl.SubUser
{
    public interface ISubSalesRepAppService : IApplicationService
    {
        Task<List<SubSalesRepDto>> GetSubSalesRep(long id);
        Task<List<User>> GetSalesRep();
        Task Update(SubSalesRepReponseDto input);
    }

}
