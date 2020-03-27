using System.Threading.Tasks;
using Abp.Application.Services;
using ServiceControl.Sessions.Dto;

namespace ServiceControl.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
