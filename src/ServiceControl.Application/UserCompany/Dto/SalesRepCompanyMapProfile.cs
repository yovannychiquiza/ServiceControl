using AutoMapper;
using ServiceControl.Authorization.Users;

namespace ServiceControl.UserCompany.Dto
{
    public class SalesRepCompanyMapProfile : Profile
    {
        public SalesRepCompanyMapProfile()
        {
            CreateMap<SalesRepCompany, SalesRepCompanyDto>();
        }
    }
}
