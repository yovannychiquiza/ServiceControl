using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceControl.Orders.Dto;
using ServiceControl.UserCompany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceControl.Orders
{
    public enum OrderStateEnum
    {
        Booked = 1,
        Cancelled = 2,
        Delayed = 3,
        To_be_follow = 4
    }
    public class OrderAppService : ApplicationService, IOrderAppService
    {

        private readonly IRepository<Orders> _orderRepository;
        private readonly IAbpSession _session;
        private readonly IRepository<SalesRepCompany> _salesRepCompanyRepository;

        public OrderAppService(IRepository<Orders> repository, IAbpSession session, IRepository<SalesRepCompany> salesRepCompanyRepository)
        {
            LocalizationSourceName = ServiceControlConsts.LocalizationSourceName;
            _orderRepository = repository;
            _session = session;
            _salesRepCompanyRepository = salesRepCompanyRepository;
        }


        public async Task<PagedOrderResultResponseDto> GetAll(PagedOrderResultRequestDto input)
        {
            var query = _orderRepository.GetAll();
                
            if (!input.Keyword.IsNullOrWhiteSpace())
                query = query.Where(x => x.Serial.Contains(input.Keyword) || x.Company.Name.Contains(input.Keyword) || x.OrderState.Name.Contains(input.Keyword));
            if (input.DateFrom.HasValue)
                query = query.Where(x => x.DateBooked >= input.DateFrom);
            if (input.DateTo.HasValue)
                query = query.Where(x => x.DateBooked <= input.DateTo);

            var ordersList = query
                .Include(t => t.OrderState)
                .Include(t => t.Company)
                .Include(t => t.SalesRep)
                .Include(t => t.TimeSlot)
                .Include(t => t.FirstIdentification)
                .Include(t => t.SecondIdentification)
                .OrderByDescending(t => t.Id)
                .ToList();

            int count = ordersList.Count();
            var newList = ordersList.Skip(input.SkipCount).Take(input.MaxResultCount);

            ListResultDto<OrderListDto> ss = new ListResultDto<OrderListDto>();

            PagedOrderResultResponseDto pagedOrderResultResponseDto = new PagedOrderResultResponseDto();
            pagedOrderResultResponseDto.TotalCount = count;
            pagedOrderResultResponseDto.Data = new ListResultDto<OrderListDto>(
                ObjectMapper.Map<List<OrderListDto>>(newList));

            return pagedOrderResultResponseDto;
        }

        public async Task Create(OrderDto input)
        {
            try
            {
                input.SalesRepId = _session.UserId.GetValueOrDefault();
                input.OrderStateId = (int)OrderStateEnum.Booked;
                input.DateBooked = DateTime.Now;
                var task = ObjectMapper.Map<Orders>(input);
                await _orderRepository.InsertAsync(task);
            }
            catch (Exception e)
            {
                string mess = e.Message;
            }
        }
        public async Task GetOrderDelete(long id)
        {
            var model = _orderRepository.FirstOrDefault(t => t.Id == id);
            _orderRepository.DeleteAsync(model);
        }
        public async Task Update(OrderDto input)
        {
            var task = ObjectMapper.Map<Orders>(input);
            await _orderRepository.UpdateAsync(task);
        }

        public Task<OrderDto> GetOrder(long id)
        {
            Orders model = _orderRepository.FirstOrDefault(t => t.Id == id);
            OrderDto dto = new OrderDto(); 
            dto = ObjectMapper.Map(model, dto);
            return Task.FromResult(dto);        
        }

        public async Task<ListResultDto<ComboboxItemDto>> GetCompanyComboboxItems(long id)
        {
            List<SalesRepCompany> list = _salesRepCompanyRepository.GetAll()
                .Include(t => t.SalesRep)
                .Include(t => t.Company)
                .Where(t => t.SalesRepId == id).ToList();
            return new ListResultDto<ComboboxItemDto>(
                list.Select(p => new ComboboxItemDto(p.Company.Id.ToString("D"), p.Company.Name)).ToList()
            );
        }

        public async Task GetOrderBooking(OrderDto input)
        {
            try
            {
                Orders model = _orderRepository.FirstOrDefault(t => t.Id == input.Id);
                model.OrderNo = input.OrderNo;
                model.AccountNo = input.AccountNo;
                model.InstallDate = input.InstallDate;
                model.Remarks = input.Remarks;
                model.OrderStateId = input.OrderStateId;
                await _orderRepository.UpdateAsync(model);
            }
            catch (Exception e)
            {
                string mess = e.Message;
            }
        }

        public Task<ExportResultResponse> GetExportExcel(PagedOrderResultRequestDto input)
        {
            var result = GetAll(input).Result.Data.Items;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Orders");

                int row = 1, col = 1;

                worksheet.Cells[row, col++].Value = L("Company");
                worksheet.Cells[row, col++].Value = L("Serial");
                worksheet.Cells[row, col++].Value = L("DateBooked");
                worksheet.Cells[row, col++].Value = L("SalesRep");
                worksheet.Cells[row, col++].Value = L("CustomerFirstName");
                worksheet.Cells[row, col++].Value = L("CustomerLastName");
                worksheet.Cells[row, col++].Value = L("ContactPhone");
                worksheet.Cells[row, col++].Value = L("Email");
                worksheet.Cells[row, col++].Value = L("DateOfBirth");
                worksheet.Cells[row, col++].Value = L("FirstIdentification");
                worksheet.Cells[row, col++].Value = L("SecondIdentification");
                worksheet.Cells[row, col++].Value = L("ExistingAccountNo");
                worksheet.Cells[row, col++].Value = L("StreetNo");
                worksheet.Cells[row, col++].Value = L("CustomerAddress");
                worksheet.Cells[row, col++].Value = L("Unit");
                worksheet.Cells[row, col++].Value = L("City");
                worksheet.Cells[row, col++].Value = L("PostalCode");
                worksheet.Cells[row, col++].Value = L("PromoDetails");
                worksheet.Cells[row, col++].Value = L("TimeSlot");
                worksheet.Cells[row, col++].Value = L("Notes");

                worksheet.Cells[ExcelRange.GetAddress(1, 1, 1, col)].Style.Font.Bold = true;

                foreach (var item in result.ToList())
                {
                    row++;
                    col = 1;
                    worksheet.Cells[row, col++].Value = item.Company.Name;
                    worksheet.Cells[row, col++].Value = item.Serial;
                    worksheet.Cells[row, col++].Value = item.DateBooked.ToString(L("DateFormat"));
                    worksheet.Cells[row, col++].Value = item.SalesRep.Name;
                    worksheet.Cells[row, col++].Value = item.CustomerFirstName;
                    worksheet.Cells[row, col++].Value = item.CustomerLastName;
                    worksheet.Cells[row, col++].Value = item.ContactPhone;
                    worksheet.Cells[row, col++].Value = item.Email;
                    worksheet.Cells[row, col++].Value = item.DateOfBirth.ToString(L("DateFormat"));
                    worksheet.Cells[row, col++].Value = item.FirstIdentification.Name;
                    worksheet.Cells[row, col++].Value = item.SecondIdentification.Name;
                    worksheet.Cells[row, col++].Value = item.ExistingAccountNo;
                    worksheet.Cells[row, col++].Value = item.StreetNo;
                    worksheet.Cells[row, col++].Value = item.CustomerAddress;
                    worksheet.Cells[row, col++].Value = item.Unit;
                    worksheet.Cells[row, col++].Value = item.City;
                    worksheet.Cells[row, col++].Value = item.PostalCode;
                    worksheet.Cells[row, col++].Value = item.PromoDetails;
                    worksheet.Cells[row, col++].Value = item.TimeSlot.Name;
                    worksheet.Cells[row, col++].Value = item.Notes;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var response = Task.FromResult( new ExportResultResponse {
                    FileName = "Orders.xlsx",
                    Data = package.GetAsByteArray()
                });

                return response;
            }
        }

    }
}
