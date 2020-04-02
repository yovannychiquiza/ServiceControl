using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Common
{
    public interface ILookupAppService : IApplicationService
    {
        Task<ListResultDto<ComboboxItemDto>> GetOrderStateComboboxItems();
        Task<ListResultDto<ComboboxItemDto>> GetCompanyComboboxItems();
        ListResultDto<ComboboxItemDto> GetExistingAccountNoItems();
        ListResultDto<ComboboxItemDto> GetIdentificationItems();
    }
}
