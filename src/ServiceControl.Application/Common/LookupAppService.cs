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
    public enum ExistingAccountNoEnum
    {
        Yes = 1,
        No = 2,
    }
    public enum IdentificationEnum
    {
        DL = 1,
        Passport = 2
    }
   

    public class LookupAppService : ApplicationService, ILookupAppService
    {
        private readonly IRepository<OrderState, int> _orderStateRepository;
        private readonly IRepository<Company, int> _companyRepository;

        public LookupAppService(
            IRepository<OrderState, int> orderStateRepository,
            IRepository<Company, int> companyRepository
            )
        {
            _orderStateRepository = orderStateRepository;
            _companyRepository = companyRepository;
        }

        public async Task<ListResultDto<ComboboxItemDto>> GetOrderStateComboboxItems()
        {
            var list = await _orderStateRepository.GetAllListAsync();
            return new ListResultDto<ComboboxItemDto>(
                list.Select(p => new ComboboxItemDto(p.Id.ToString("D"), p.Name)).ToList()
            );
        }
        public async Task<ListResultDto<ComboboxItemDto>> GetCompanyComboboxItems()
        {
            var list = await _companyRepository.GetAllListAsync();
            return new ListResultDto<ComboboxItemDto>(
                list.Select(p => new ComboboxItemDto(p.Id.ToString("D"), p.Name)).ToList()
            );
        }
        public ListResultDto<ComboboxItemDto> GetExistingAccountNoItems()
        {
            var list = new List<ComboboxItemDto>
            {
                new ComboboxItemDto { DisplayText = ExistingAccountNoEnum.Yes.ToString(), Value = ExistingAccountNoEnum.Yes.ToString()},
                new ComboboxItemDto { DisplayText = ExistingAccountNoEnum.No.ToString(), Value = ExistingAccountNoEnum.No.ToString()}
            };

            ListResultDto<ComboboxItemDto> lis = new ListResultDto<ComboboxItemDto>();
            lis.Items = list;
            return lis;
        }
        public ListResultDto<ComboboxItemDto> GetIdentificationItems()
        {
            var list = new List<ComboboxItemDto>
            {
                new ComboboxItemDto { DisplayText = IdentificationEnum.DL.ToString(), Value = IdentificationEnum.DL.ToString()},
                new ComboboxItemDto { DisplayText = IdentificationEnum.Passport.ToString(), Value = IdentificationEnum.Passport.ToString()}
            };

            ListResultDto<ComboboxItemDto> lis = new ListResultDto<ComboboxItemDto>();
            lis.Items = list;
            return lis;
        }

    }
}
