using Abp.Extensions;
using AutoMapper;
using ServiceControl.Authorization.Users;
using System;

namespace ServiceControl.Orders.Dto
{
    public class OrderMapProfile : Profile
    {
        public OrderMapProfile()
        {
            CreateMap<OrderDto, Orders>();
            CreateMap<Orders, OrderDto>();
            CreateMap<ProductType, ProductTypeDto>();
            CreateMap<OrdersProductType, OrdersProductTypeDto>();
            CreateMap<DateTime?, string>().ConvertUsing(new DateTimeTypeConverter());
            CreateMap<string, DateTime?>().ConvertUsing(new DateTimeStringTypeConverter());
            CreateMap<DateTime, string>().ConvertUsing(s => s.ToString(AppConsts.DateFormat));
        }
    }
    public class DateTimeTypeConverter : ITypeConverter<DateTime?, string>
    {
        public string Convert(DateTime? source, string destination, ResolutionContext context)
        {
            return source.HasValue ? source.GetValueOrDefault().ToString(AppConsts.DateTimeFormat) : null;   
        }
    }
    public class DateTimeStringTypeConverter : ITypeConverter<string, DateTime?>
    {
        public DateTime? Convert(String source, DateTime? destination, ResolutionContext context)
        {
            if (!source.IsNullOrEmpty())
            {
                return DateTime.Parse(source);
            }
            else
            {
                return null;
            }
        }
    }
}
