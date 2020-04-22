using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceControl.Authorization;
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
        Created = 1,
        Booked = 2,
        Cancelled = 3,
        Delayed = 4,
        Follow = 5
    }

    [AbpAuthorize(PermissionNames.Pages_Orders, PermissionNames.Pages_Booking)]
    public class OrderAppService : ApplicationService, IOrderAppService
    {

        private readonly IRepository<Orders> _orderRepository;
        private readonly IAbpSession _session;
        private readonly IRepository<SalesRepCompany> _salesRepCompanyRepository;
        private readonly IRepository<SalesRepSerial> _salesRepSerialRepository;


        public OrderAppService(IRepository<Orders> repository, 
            IAbpSession session, 
            IRepository<SalesRepCompany> salesRepCompanyRepository,
            IRepository<SalesRepSerial> salesRepSerialRepository
            )
        {
            LocalizationSourceName = ServiceControlConsts.LocalizationSourceName;
            _orderRepository = repository;
            _session = session;
            _salesRepCompanyRepository = salesRepCompanyRepository;
            _salesRepSerialRepository = salesRepSerialRepository;
        }


        public async Task<PagedOrderResultResponseDto> GetAll(PagedOrderResultRequestDto input)
        {
            var userId =_session.UserId.GetValueOrDefault();
            var query = _orderRepository.GetAll();
            //////Filters by page
            if (input.DateFrom.HasValue)//Filter by DateFrom
                query = query.Where(x => x.DateBooked.Date >= input.DateFrom.Value.Date);
            if (input.DateTo.HasValue)//Filter by DateTo
                query = query.Where(x => x.DateBooked.Date <= input.DateTo.Value.Date);
           
            if (!input.Followed.IsNullOrEmpty())//Filter by Followed
                query = query.Where(x => x.Followed == input.Followed);

            int[] arrayCompany = ConvertToArrayInt(input.CompanyId);//Filter by company
            if (arrayCompany.Length >= 1)
                query = query.Where(x => arrayCompany.Contains(x.CompanyId));

            int[] arrayOrderState = ConvertToArrayInt(input.OrderStateId);//Filter by order state
            if (arrayOrderState.Length >= 1)
                query = query.Where(x => arrayOrderState.Contains(x.OrderStateId));
            //////Filters by page

            var permissionOrderSeeAll = PermissionChecker.IsGranted(PermissionNames.Order_See_All); //validation for see all orders
            if (!permissionOrderSeeAll)
            {
                query = query.Where(x => x.SalesRepId == userId);
            }

            var listCompany = _salesRepCompanyRepository.GetAll()//validation for see only company assigned
               .Where(t => t.SalesRepId == userId).Select(t => t.Company.Id).ToArray();
            query = query.Where(x => listCompany.Contains(x.Company.Id));

            var permissionOrderReady = PermissionChecker.IsGranted(PermissionNames.Order_Ready);
            var permissionOrderAdminReady = PermissionChecker.IsGranted(PermissionNames.Order_Admin_Ready);

            if (permissionOrderReady && !permissionOrderAdminReady)//validation for order ready to booking
            {
                query = query.Where(x => x.IsReady == true);
            }

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

        public int[] ConvertToArrayInt(string data) {
            List<int> arrayInt = new List<int>();
            if (data != null)
            {
                var array =  data.Split(",");
                foreach (var item in array)
                {
                    if (!item.IsNullOrWhiteSpace())
                    {
                        arrayInt.Add(Int32.Parse(item));
                    }
                }
            }
            return arrayInt.ToArray();
        }

        public async Task Create(OrderDto input)
        {
            input.SalesRepId = _session.UserId.GetValueOrDefault();
            input.OrderStateId = (int)OrderStateEnum.Created;
            input.DateBooked = DateTime.Now;

            var salesRepCompany = _salesRepCompanyRepository.FirstOrDefault(t => t.CompanyId == input.CompanyId && t.SalesRepId == input.SalesRepId);
            input.Sgi = salesRepCompany.Code != null ? salesRepCompany.Code : "";//assing Sgi code

            var order = ObjectMapper.Map<Orders>(input);
            order.Serial = string.Empty;
            var orderId = await _orderRepository.InsertAndGetIdAsync(order);
            order.Serial = GetSerial();
            await _orderRepository.UpdateAsync(order);
        }
        /// <summary>
        /// return serial number by user
        /// </summary>
        /// <returns></returns>
        public string GetSerial()
        {
            string serial = String.Empty;
            var salesRepSerial = _salesRepSerialRepository.FirstOrDefault(t => t.SalesRepId == _session.UserId.GetValueOrDefault());
            if (salesRepSerial != null)///if serial by user exist then get next serial
            {
                salesRepSerial.Serial = salesRepSerial.Serial + 1;
                serial = salesRepSerial.Serial.ToString();
                _salesRepSerialRepository.Update(salesRepSerial);
            }
            else///if serial by user does not exist then create a new one
            {
                serial = "1";
                SalesRepSerial salesRepSerial1 = new SalesRepSerial();
                salesRepSerial1.SalesRepId = _session.UserId.GetValueOrDefault();
                salesRepSerial1.Serial = 1;
                _salesRepSerialRepository.Insert(salesRepSerial1);
            }
            return serial;
        }

        public async Task GetOrderDelete(long id)
        {
            var model = _orderRepository.FirstOrDefault(t => t.Id == id);
            await _orderRepository.DeleteAsync(model);
        }
        public async Task Update(OrderDto input)
        {
            var order = ObjectMapper.Map<Orders>(input);
            await _orderRepository.UpdateAsync(order);
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
                model.InstallDate = DateTime.Parse(input.InstallDate);
                model.Remarks = input.Remarks;
                model.OrderStateId = input.OrderStateId;
                await _orderRepository.UpdateAsync(model);
            }
            catch (Exception e)
            {
                string mess = e.Message;
            }
        }
        public async Task GetBookingUpdate(OrderDto input)
        {
            Orders model = _orderRepository.FirstOrDefault(t => t.Id == input.Id);
            model.CustomerFirstName = input.CustomerFirstName;
            model.CustomerLastName = input.CustomerLastName;
            model.ContactPhone = input.ContactPhone;
            model.Email = input.Email;
            model.DateOfBirth = input.DateOfBirth;
            model.ExistingAccountNo = input.ExistingAccountNo;
            model.StreetNo = input.StreetNo;
            model.CustomerAddress = input.CustomerAddress;
            model.Unit = input.Unit;
            model.City = input.City;
            model.PostalCode = input.PostalCode;
            model.PromoDetails = input.PromoDetails;
            model.Notes = input.Notes;
            model.OrderNo = input.OrderNo;
            model.AccountNo = input.AccountNo;
            model.Remarks = input.Remarks;
            model.IsReady = Boolean.Parse(input.IsReady);
            if (input.InstallDate.IsNullOrEmpty()){ 
                model.InstallDate = null;
            }
            else
            {
                model.InstallDate = DateTime.Parse(input.InstallDate);
            }

            if (OrderStateEnum.Cancelled.ToString().Equals(input.OrderStateName))
            {
                model.Followed = input.Followed;
                model.Explanation = input.Explanation;
            }
            if (OrderStateEnum.Booked.ToString().Equals(input.OrderStateName))
                model.OrderStateId = (int)OrderStateEnum.Booked;
            if (OrderStateEnum.Cancelled.ToString().Equals(input.OrderStateName))
                model.OrderStateId = (int)OrderStateEnum.Cancelled;
            if (OrderStateEnum.Delayed.ToString().Equals(input.OrderStateName))
                model.OrderStateId = (int)OrderStateEnum.Delayed;
            if (OrderStateEnum.Follow.ToString().Equals(input.OrderStateName))
                model.OrderStateId = (int)OrderStateEnum.Follow;

            await _orderRepository.UpdateAsync(model);
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
                worksheet.Cells[row, col++].Value = L("OrderNo");
                worksheet.Cells[row, col++].Value = L("AccountNo");
                worksheet.Cells[row, col++].Value = L("InstallDate");
                worksheet.Cells[row, col++].Value = L("OrderState");
                worksheet.Cells[row, col++].Value = L("Remarks");
                worksheet.Cells[row, col++].Value = L("Followed");
                worksheet.Cells[row, col++].Value = L("Explanation");

                worksheet.Cells[ExcelRange.GetAddress(1, 1, 1, col)].Style.Font.Bold = true;

                foreach (var item in result.ToList())
                {
                    row++;
                    col = 1;
                    worksheet.Cells[row, col++].Value = item.Company.Name;
                    worksheet.Cells[row, col++].Value = item.Serial;
                    worksheet.Cells[row, col++].Value = item.DateBooked.ToString(AppConsts.DateFormat);
                    worksheet.Cells[row, col++].Value = item.SalesRep.Name;
                    worksheet.Cells[row, col++].Value = item.CustomerFirstName;
                    worksheet.Cells[row, col++].Value = item.CustomerLastName;
                    worksheet.Cells[row, col++].Value = item.ContactPhone;
                    worksheet.Cells[row, col++].Value = item.Email;
                    worksheet.Cells[row, col++].Value = item.DateOfBirth.ToString(AppConsts.DateFormat);
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
                    worksheet.Cells[row, col++].Value = item.OrderNo;
                    worksheet.Cells[row, col++].Value = item.AccountNo;
                    worksheet.Cells[row, col++].Value = item.InstallDate;
                    worksheet.Cells[row, col++].Value = item.OrderState.Name;
                    worksheet.Cells[row, col++].Value = item.Remarks;
                    worksheet.Cells[row, col++].Value = item.Followed;
                    worksheet.Cells[row, col++].Value = item.Explanation;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var response = Task.FromResult(new ExportResultResponse {
                    FileName = "Orders.xlsx",
                    Data = package.GetAsByteArray()
                });

                return response;
            }
        }
    }
}
