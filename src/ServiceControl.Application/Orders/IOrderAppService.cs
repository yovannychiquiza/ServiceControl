using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ServiceControl.Orders.Dto;
using System.Threading.Tasks;

namespace ServiceControl.Orders
{
    public interface IOrderAppService : IApplicationService
    {
        Task<PagedOrderResultResponseDto> GetAll(PagedOrderResultRequestDto input);
        Task Create(OrderDto input);
        Task GetOrderDelete(long id);
        Task<OrderDto> GetOrder(long id);
        Task<ExportResultResponse> GetExportExcel(PagedOrderResultRequestDto input);
        Task<ListResultDto<ComboboxItemDto>> GetCompanyComboboxItems(long id);
        Task GetOrderBooking(OrderDto input);

    }

}
