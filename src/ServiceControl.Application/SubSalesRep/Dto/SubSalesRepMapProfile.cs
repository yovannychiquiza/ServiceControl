using AutoMapper;
using ServiceControl.Authorization.Users;
using ServiceControl.Orders;

namespace ServiceControl.SubUser.Dto
{
    public class SubSalesRepMapProfile : Profile
    {
        public SubSalesRepMapProfile()
        {
            CreateMap<SubSalesRep, SubSalesRepDto>();
        }
    }
}
