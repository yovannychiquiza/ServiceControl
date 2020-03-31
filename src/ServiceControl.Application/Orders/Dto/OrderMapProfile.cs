using AutoMapper;
using ServiceControl.Authorization.Users;

namespace ServiceControl.Orders.Dto
{
    public class OrderMapProfile : Profile
    {
        public OrderMapProfile()
        {
            CreateMap<OrderDto, Orders>();
            CreateMap<Orders, OrderDto>();
        }
    }
}
