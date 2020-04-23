using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceControl.Orders;
using Abp.Authorization;
using ServiceControl.Authorization;
using ServiceControl.Authorization.Users;
using ServiceControl.SubUser.Dto;
using Microsoft.EntityFrameworkCore;

namespace ServiceControl.SubUser
{
    [AbpAuthorize(PermissionNames.Pages_Orders)]
    public class SubSalesRepAppService : ApplicationService, ISubSalesRepAppService
    {
        private readonly UserManager _userManager;
        private readonly IRepository<SubSalesRep> _subSalesRepRepository;
        private readonly IRepository<User, long> _repositoryUser;

        public SubSalesRepAppService(IRepository<SubSalesRep> repository,
            UserManager userManager,
            IRepository<User, long> repositoryUser
            )
        {
            LocalizationSourceName = ServiceControlConsts.LocalizationSourceName;
            _subSalesRepRepository = repository;
            _userManager = userManager;
            _repositoryUser = repositoryUser;
        }

        public async Task<List<SubSalesRepDto>> GetSubSalesRep(long id)
        {
            List<SubSalesRep> model = _subSalesRepRepository.GetAll().Where(t => t.SalesRepId == id).ToList();
            var response = new List<SubSalesRepDto>(
                ObjectMapper.Map<List<SubSalesRepDto>>(model)
            );

            return response;
        }

        public async Task<List<User>> GetSalesRep()
        {
            var list = await _repositoryUser.GetAllListAsync();
            return list;
        }

        public async Task Update(SubSalesRepReponseDto input)
        {
            List<User> salesRep = await GetSalesRep();

            foreach (var item in salesRep)
            {
                bool exist = false;
                foreach (var sales in input.SubSalesRepList)
                {
                    if (item.Id == sales.SubSalesRepId) { exist = true; break; }
                }

                if (exist)//if salesRep is selected
                {
                    var existSubSalesRep = _subSalesRepRepository.GetAll().Where(t => t.SalesRepId == input.Id
                    && t.SubSalesRepId == item.Id);
                    if (!existSubSalesRep.Any())//if salesRep is not saved
                    {
                        SubSalesRep subSalesRep = new SubSalesRep();
                        subSalesRep.SubSalesRepId = item.Id;
                        subSalesRep.SalesRepId = input.Id;
                        await _subSalesRepRepository.InsertAsync(subSalesRep);//create 
                    }
                }
                else//if salesRep is not selected
                {
                    var existSubSalesRep = _subSalesRepRepository.GetAll().Where(t => t.SalesRepId == input.Id
                    && t.SubSalesRepId == item.Id);
                    if (existSubSalesRep.Any())//if salesRep is saved
                    {
                        await _subSalesRepRepository.DeleteAsync(existSubSalesRep.First());//delete
                    }
                }
            }
        }

        public async Task<ListResultDto<ComboboxItemDto>> GetSubSalesRepComboboxItems(long id)
        {
            List<SubSalesRep> list = _subSalesRepRepository.GetAll()
                .Include(t => t.SalesRep)
                .Include(t => t.SubSalesRepr)
                .Where(t => t.SalesRepId == id).ToList();
            return new ListResultDto<ComboboxItemDto>(
                list.Select(p => new ComboboxItemDto(p.SubSalesRepr.Id.ToString("D"), p.SubSalesRepr.Name)).ToList()
            );
        }
    }
}
