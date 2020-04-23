using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ServiceControl.Authorization.Users;
using ServiceControl.Orders;

namespace ServiceControl.SubUser.Dto
{
    [AutoMapTo(typeof(SubSalesRep))]
    public class SubSalesRepDto : EntityDto<long>
    {
        public long SalesRepId { get; set; }
        public User SalesRep { get; set; }
        public long SubSalesRepId { get; set; }
        public User SubSalesRepr { get; set; }
    }
}
