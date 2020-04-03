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
   
   

    public class LookupAppService : ApplicationService, ILookupAppService
    {
        private readonly IRepository<OrderState, int> _orderStateRepository;
        private readonly IRepository<Company, int> _companyRepository;
        private readonly IRepository<FirstIdentification, int> _firstIdentificationRepository;
        private readonly IRepository<SecondIdentification, int> _secondIdentificationRepository;
        private readonly IRepository<TimeSlot, int> _timeSlotRepository;

        public LookupAppService(
            IRepository<OrderState, int> orderStateRepository,
            IRepository<Company, int> companyRepository,
            IRepository<FirstIdentification, int> firstIdentificationRepository,
            IRepository<SecondIdentification, int> secondIdentificationRepository,
            IRepository<TimeSlot, int> timeSlotRepository
            )
        {
            _orderStateRepository = orderStateRepository;
            _companyRepository = companyRepository;
            _firstIdentificationRepository = firstIdentificationRepository;
            _secondIdentificationRepository = secondIdentificationRepository;
            _timeSlotRepository = timeSlotRepository;
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
        public async Task<ListResultDto<ComboboxItemDto>> GetTimeSlotComboboxItems()
        {
            var list = await _timeSlotRepository.GetAllListAsync();
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

        public async Task<ListResultDto<ComboboxItemDto>> GetFirstIdentificationItems()
        {
            var list = await _firstIdentificationRepository.GetAllListAsync();
            return new ListResultDto<ComboboxItemDto>(
                list.Select(p => new ComboboxItemDto(p.Id.ToString("D"), p.Name)).ToList()
            );
        }
        public async Task<ListResultDto<ComboboxItemDto>> GetSecondIdentificationItems()
        {
            var list = await _secondIdentificationRepository.GetAllListAsync();
            return new ListResultDto<ComboboxItemDto>(
                list.Select(p => new ComboboxItemDto(p.Id.ToString("D"), p.Name)).ToList()
            );
        }

       

    }
}
