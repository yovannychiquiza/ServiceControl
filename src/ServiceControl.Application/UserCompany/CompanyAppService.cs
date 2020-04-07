using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceControl.UserCompany.Dto;
using ServiceControl.Orders;
using System;
using Abp.Authorization;
using ServiceControl.Authorization;

namespace ServiceControl.UserCompany
{
    [AbpAuthorize(PermissionNames.Pages_Orders)]
    public class CompanyAppService : ApplicationService, ICompanyAppService
    {

        private readonly IRepository<SalesRepCompany> _salesRepCompanyRepository;
        private readonly IRepository<Company, int> _companyRepository;

        public CompanyAppService(IRepository<SalesRepCompany> repository, 
            IRepository<Company, int> companyRepository
            )
        {
            LocalizationSourceName = ServiceControlConsts.LocalizationSourceName;
            _salesRepCompanyRepository = repository;
            _companyRepository = companyRepository;
        }

        public async Task<List<SalesRepCompanyDto>> GetSalesRepCompany(long id)
        {
            List<SalesRepCompany> model = _salesRepCompanyRepository.GetAll().Where(t => t.SalesRepId == id).ToList();
            SalesRepCompanyDto dto = new SalesRepCompanyDto();

            var response = new List<SalesRepCompanyDto>(
            ObjectMapper.Map<List<SalesRepCompanyDto>>(model)
            );

            return response;
        }

        public async Task<List<Company>> GetCompany()
        {
            var list = await _companyRepository.GetAllListAsync();
            return list;
        }

        public async Task Update(CompanyReponseDto input)
        {
            List<Company> companies = await GetCompany();

            foreach (var item in companies)
            {
                bool exist = false;
                foreach (var companyView in input.CompanyList)
                {
                    if (item.Id == int.Parse(companyView)){ exist = true; }
                }

                if (exist)//if company is selected
                {
                    var existsalesRepCompany = _salesRepCompanyRepository.GetAll().Where(t => t.SalesRepId == input.Id
                    && t.CompanyId == item.Id);
                    if (!existsalesRepCompany.Any())//if company is not saved
                    {
                        SalesRepCompany salesRepCompany = new SalesRepCompany();
                        salesRepCompany.CompanyId = item.Id;
                        salesRepCompany.SalesRepId = input.Id;
                        await _salesRepCompanyRepository.InsertAsync(salesRepCompany);//create 
                    }
                }
                else//if company is not selected
                {
                    var existsalesRepCompany = _salesRepCompanyRepository.GetAll().Where(t => t.SalesRepId == input.Id
                    && t.CompanyId == item.Id);
                    if (existsalesRepCompany.Any())//if company is saved
                    {
                        await _salesRepCompanyRepository.DeleteAsync(existsalesRepCompany.First());//delete
                    }
                }
            }
        }
    }
}
