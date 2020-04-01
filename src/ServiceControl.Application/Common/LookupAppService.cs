using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using ServiceControl.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Common
{
    public class LookupAppService : ApplicationService, ILookupAppService
    {
        private readonly IRepository<OrderState, int> _orderStateRepository;

        public LookupAppService(IRepository<OrderState, int> orderStateRepository)
        {
            _orderStateRepository = orderStateRepository;
        }

        public async Task<ListResultDto<ComboboxItemDto>> GetOrderStateComboboxItems()
        {
            var list = await _orderStateRepository.GetAllListAsync();
            return new ListResultDto<ComboboxItemDto>(
                list.Select(p => new ComboboxItemDto(p.Id.ToString("D"), p.Name)).ToList()
            );
        }

    }
}
