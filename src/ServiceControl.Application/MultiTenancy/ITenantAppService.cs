using Abp.Application.Services;
using ServiceControl.MultiTenancy.Dto;

namespace ServiceControl.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

