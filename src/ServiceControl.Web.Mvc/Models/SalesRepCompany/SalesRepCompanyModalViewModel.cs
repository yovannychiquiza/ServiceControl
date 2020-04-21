using System.Collections.Generic;
using System.Linq;
using ServiceControl.Orders;
using ServiceControl.UserCompany.Dto;
using ServiceControl.Users.Dto;

namespace ServiceControl.Web.Models.Users
{
    public class SalesRepCompanyModalViewModel
    {
        public UserDto User { get; set; }

        public List<SalesRepCompanyDto> SalesRepCompanyDto { get; set; }
        public List<Company> Company { get; set; }


        public bool UserIsInCompany(Company company)
        {
            return SalesRepCompanyDto != null && SalesRepCompanyDto.Any(r => r.Company.Id == company.Id);
        }
        public string UserIsInCompanyCode(Company company)
        {
            string code = "";
            if (SalesRepCompanyDto != null && SalesRepCompanyDto.Count > 0)
            {
                var model = SalesRepCompanyDto.Where(r => r.Company.Id == company.Id).FirstOrDefault();
                code = model != null ? model.Code : null;
            }
            return code;
        }
    }
}
