using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Session;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceControl.Authorization;
using ServiceControl.Orders.Dto;
using ServiceControl.UserCompany;
using ServiceControl.Validation;
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
        Follow = 5,
        Disconnected = 6
    }
    public enum PaymentStatusEnum
    {
        Pending = 1,
        Done = 2,
        Deduction = 3
    }

    [AbpAuthorize(PermissionNames.Pages_Orders, PermissionNames.Pages_Booking)]
    public class OrderAppService : ApplicationService, IOrderAppService
    {

        private readonly IRepository<Orders> _orderRepository;
        private readonly IAbpSession _session;
        private readonly IRepository<SalesRepCompany> _salesRepCompanyRepository;
        private readonly IRepository<SalesRepSerial> _salesRepSerialRepository;
        private readonly IRepository<SubSalesRep> _subSalesRepRepository;
        private readonly IRepository<ProductType, int> _productTypeRepository;
        private readonly IRepository<OrdersProductType, int> _orders_ProductTypeRepository;
        private readonly IRepository<PaymentStatus, int> _paymentStatusRepository;
        private readonly IRepository<OrderState, int> _orderStateRepository;
        

        public OrderAppService(IRepository<Orders> repository, 
            IAbpSession session, 
            IRepository<SalesRepCompany> salesRepCompanyRepository,
            IRepository<SalesRepSerial> salesRepSerialRepository,
            IRepository<SubSalesRep> subSalesRepRepository,
            IRepository<ProductType, int> productTypeRepository,
            IRepository<OrdersProductType, int> orders_ProductTypeRepository,
            IRepository<PaymentStatus, int> paymentStatusRepository,
            IRepository<OrderState, int> orderStateRepository
            )
        {
            LocalizationSourceName = ServiceControlConsts.LocalizationSourceName;
            _orderRepository = repository;
            _session = session;
            _salesRepCompanyRepository = salesRepCompanyRepository;
            _salesRepSerialRepository = salesRepSerialRepository;
            _subSalesRepRepository = subSalesRepRepository;
            _productTypeRepository = productTypeRepository;
            _orders_ProductTypeRepository = orders_ProductTypeRepository;
            _paymentStatusRepository = paymentStatusRepository;
            _orderStateRepository = orderStateRepository;
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

            if (!input.Sgi.IsNullOrEmpty())//Filter by Sgi
                query = query.Where(x => x.Sgi == input.Sgi);
            if (!input.InvoiceNo.IsNullOrEmpty())//Filter by InvoiceNo
                query = query.Where(x => x.InvoiceNo == input.InvoiceNo);
            if (input.PaymentStatusId != 0)//Filter by InvoiceNo
                query = query.Where(x => x.PaymentStatusId == input.PaymentStatusId);

            //////Filters by page

            var permissionOrderSeeAll = PermissionChecker.IsGranted(PermissionNames.Order_See_All); //validation for see all orders
            if (!permissionOrderSeeAll)
            {
                var listSubSalesRep = _subSalesRepRepository.GetAll()//validation for sub salesRep
                .Where(t => t.SalesRepId == userId).Select(t => t.SubSalesRepr.Id).ToList();
                if(listSubSalesRep.Count() >= 1)
                {
                    listSubSalesRep.Add(userId);
                    query = query.Where(x => listSubSalesRep.Contains(x.SalesRepId));
                }
                else
                {
                    query = query.Where(x => x.SalesRepId == userId);
                }
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
                .Include(t => t.OrdersProductType)
                .Include(t => t.PaymentStatus)
                .OrderByDescending(t => t.Id)
                .ToList();

            int count = ordersList.Count();
            var newList = ordersList.Skip(input.SkipCount).Take(input.MaxResultCount);

            var product = _productTypeRepository.GetAll().ToList();
            List<ProductTypeDto> productTypeDto = new List<ProductTypeDto>();
            productTypeDto = ObjectMapper.Map(product, productTypeDto);

            PagedOrderResultResponseDto pagedOrderResultResponseDto = new PagedOrderResultResponseDto();
            pagedOrderResultResponseDto.TotalCount = count;
            pagedOrderResultResponseDto.ProductType = productTypeDto;
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
            TextFormat(input);

            input.SalesRepId = input.SubSalesRepId != null ? input.SubSalesRepId.Value : _session.UserId.GetValueOrDefault();
            input.OrderStateId = (int)OrderStateEnum.Created;
            input.PaymentStatusId = (int)PaymentStatusEnum.Pending;
            input.DateBooked = DateTime.Now;

            var salesRepCompany = _salesRepCompanyRepository.FirstOrDefault(t => t.CompanyId == input.CompanyId && t.SalesRepId == input.SalesRepId);
            input.Sgi = salesRepCompany != null && salesRepCompany.Code != null ? salesRepCompany.Code : "";//assing Sgi code

            var order = ObjectMapper.Map<Orders>(input);
            order.Serial = string.Empty;
            var orderId = await _orderRepository.InsertAndGetIdAsync(order);
            order.Serial = GetSerial(input.SalesRepId);
            await _orderRepository.UpdateAsync(order);
            input.Id = orderId;
            await ProductType(input);
        }
 
        public async Task ProductType(OrderDto orderDto)
        {
            var productType = _productTypeRepository.GetAll().ToList();

            foreach (var item in productType)
            {
                bool isSelected = false;
                foreach (var product in orderDto.ProductTypeId.Split(","))
                {
                    if (item.Id.ToString() == product) { isSelected = true; break; }
                }

                if (isSelected)//if product is selected
                {
                    var existProduct = _orders_ProductTypeRepository.GetAll().Where(t => t.OrdersId == orderDto.Id
                    && t.ProductTypeId == item.Id);
                    if (!existProduct.Any())//if product is not saved
                    {
                        OrdersProductType orders_ProductType = new OrdersProductType();
                        orders_ProductType.OrdersId = Int32.Parse(orderDto.Id.ToString());
                        orders_ProductType.ProductTypeId = item.Id;
                        await _orders_ProductTypeRepository.InsertAsync(orders_ProductType);
                    }
                }
                else//if product is not selected
                {
                    var existProduct = _orders_ProductTypeRepository.GetAll().Where(t => t.OrdersId == orderDto.Id
                    && t.ProductTypeId == item.Id);
                    if (existProduct.Any())//if product is saved
                    {
                        await _orders_ProductTypeRepository.DeleteAsync(existProduct.First());//delete
                    }
                }
            }
        }
        /// <summary>
        /// return serial number by user
        /// </summary>
        /// <returns></returns>
        public string GetSerial(long salesRepId)
        {
            string serial = String.Empty;
            var salesRepSerial = _salesRepSerialRepository.FirstOrDefault(t => t.SalesRepId == salesRepId);
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
                salesRepSerial1.SalesRepId = salesRepId;
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
        public OrderDto TextFormat(OrderDto input)
        {
            input.CustomerFirstName = ValidationHelper.ToCapital(input.CustomerFirstName);
            input.CustomerLastName = ValidationHelper.ToCapital(input.CustomerLastName);
            input.CustomerAddress = ValidationHelper.ToCapital(input.CustomerAddress);
            input.Unit = ValidationHelper.ToCapital(input.Unit);
            input.City = ValidationHelper.ToCapital(input.City);
            input.Email = ValidationHelper.ToLower(input.Email);
            input.PostalCode = ValidationHelper.ToUpper(input.PostalCode);
            return input;
        }

        public async Task Update(OrderDto input)
        {
            TextFormat(input);
            var order = ObjectMapper.Map<Orders>(input);
            if (order.PaymentStatusId == (int)PaymentStatusEnum.Deduction)
                order.OrderStateId = (int)OrderStateEnum.Disconnected;

            await ProductType(input);
            await _orderRepository.UpdateAsync(order);
        }

        public Task<OrderDto> GetOrder(long id)
        {
            Orders model = _orderRepository.GetAll()
                .Include(t => t.OrdersProductType)
                .FirstOrDefault(t => t.Id == id);
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
            TextFormat(input);
            Orders model = _orderRepository.FirstOrDefault(t => t.Id == input.Id);
            model.CustomerFirstName = input.CustomerFirstName;
            model.CustomerLastName = input.CustomerLastName;
            model.ContactPhone = input.ContactPhone;
            model.Email = input.Email;
            model.DateOfBirth = DateTime.Parse(input.DateOfBirth);
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
            if (OrderStateEnum.Disconnected.ToString().Equals(input.OrderStateName))
                model.OrderStateId = (int)OrderStateEnum.Disconnected;

            if (PermissionChecker.IsGranted(PermissionNames.Order_Admin_Invoice))
            {
                model.InvoiceNo = input.InvoiceNo;
                if (PaymentStatusEnum.Done.ToString().Equals(input.PaymentStatusName))
                    model.PaymentStatusId = (int)PaymentStatusEnum.Done;
                if (PaymentStatusEnum.Deduction.ToString().Equals(input.PaymentStatusName))
                    model.PaymentStatusId = (int)PaymentStatusEnum.Deduction;
                if (model.PaymentStatusId == (int)PaymentStatusEnum.Deduction)
                    model.OrderStateId = (int)OrderStateEnum.Disconnected;
            }

            if (PermissionChecker.IsGranted(PermissionNames.Order_Admin_Ready))
            {
                model.IsReady = Boolean.Parse(input.IsReady);
            }

            await _orderRepository.UpdateAsync(model);
        }

        public Task<ExportResultResponse> GetExportExcel(PagedOrderResultRequestDto input)
        {
            var result = GetAll(input).Result.Data.Items;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var products = _productTypeRepository.GetAll().ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Orders");

                int row = 1, col = 1;

                worksheet.Cells[row, col++].Value = L("Company");
                worksheet.Cells[row, col++].Value = L("Serial");
                worksheet.Cells[row, col++].Value = L("DateBooked");
                worksheet.Cells[row, col++].Value = L("Sgi");
                worksheet.Cells[row, col++].Value = L("SalesRep");
                worksheet.Cells[row, col++].Value = L("RepEmail");
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

                foreach (var product in products)
                {
                    worksheet.Cells[row, col++].Value = product.Name;
                }
                worksheet.Cells[row, col++].Value = L("TimeSlot");
                worksheet.Cells[row, col++].Value = L("Notes");
                worksheet.Cells[row, col++].Value = L("OrderNo");
                worksheet.Cells[row, col++].Value = L("AccountNo");
                worksheet.Cells[row, col++].Value = L("InstallDate");
                worksheet.Cells[row, col++].Value = L("OrderState");
                worksheet.Cells[row, col++].Value = L("Remarks");
                worksheet.Cells[row, col++].Value = L("Followed");
                worksheet.Cells[row, col++].Value = L("Explanation");
                if (PermissionChecker.IsGranted(PermissionNames.Order_Admin_Invoice))
                {
                    worksheet.Cells[row, col++].Value = L("PaymentStatus");
                    worksheet.Cells[row, col++].Value = L("InvoiceNo");
                }
                if (PermissionChecker.IsGranted(PermissionNames.Order_Admin_Ready))
                {
                    worksheet.Cells[row, col++].Value = L("IsReady");
                }

                worksheet.Cells[ExcelRange.GetAddress(1, 1, 1, col)].Style.Font.Bold = true;

                foreach (var item in result.ToList())
                {
                    row++;
                    col = 1;
                    worksheet.Cells[row, col++].Value = item.Company.Name;
                    worksheet.Cells[row, col++].Value = item.Serial;
                    worksheet.Cells[row, col++].Value = item.DateBooked;
                    worksheet.Cells[row, col++].Value = item.Sgi;
                    worksheet.Cells[row, col++].Value = item.SalesRep.Name;
                    worksheet.Cells[row, col++].Value = item.SalesRep.EmailAddress;
                    worksheet.Cells[row, col++].Value = item.CustomerFirstName;
                    worksheet.Cells[row, col++].Value = item.CustomerLastName;
                    worksheet.Cells[row, col++].Value = item.ContactPhone;
                    worksheet.Cells[row, col++].Value = item.Email;
                    worksheet.Cells[row, col++].Value = item.DateOfBirth;
                    worksheet.Cells[row, col++].Value = item.FirstIdentification.Name;
                    worksheet.Cells[row, col++].Value = item.SecondIdentification.Name;
                    worksheet.Cells[row, col++].Value = item.ExistingAccountNo;
                    worksheet.Cells[row, col++].Value = item.StreetNo;
                    worksheet.Cells[row, col++].Value = item.CustomerAddress;
                    worksheet.Cells[row, col++].Value = item.Unit;
                    worksheet.Cells[row, col++].Value = item.City;
                    worksheet.Cells[row, col++].Value = item.PostalCode;
                    worksheet.Cells[row, col++].Value = item.PromoDetails;

                    var dic = item.OrdersProductType.ToList().ToDictionary(t => t.ProductTypeId, t => t.ProductTypeId);
                    foreach (var product in products)
                    {
                        worksheet.Cells[row, col++].Value = dic.ContainsKey(product.Id) ? "1": "0";
                    }
                    worksheet.Cells[row, col++].Value = item.TimeSlot.Name;
                    worksheet.Cells[row, col++].Value = item.Notes;
                    worksheet.Cells[row, col++].Value = item.OrderNo;
                    worksheet.Cells[row, col++].Value = item.AccountNo;
                    worksheet.Cells[row, col++].Value = item.InstallDate;
                    worksheet.Cells[row, col++].Value = item.OrderState.Name;
                    worksheet.Cells[row, col++].Value = item.Remarks;
                    worksheet.Cells[row, col++].Value = item.Followed;
                    worksheet.Cells[row, col++].Value = item.Explanation;
                    if (PermissionChecker.IsGranted(PermissionNames.Order_Admin_Invoice))
                    {
                        worksheet.Cells[row, col++].Value = item.PaymentStatus.Name;
                        worksheet.Cells[row, col++].Value = item.InvoiceNo;
                    }

                    if (PermissionChecker.IsGranted(PermissionNames.Order_Admin_Ready))
                    {
                        worksheet.Cells[row, col++].Value = item.IsReady;
                    }

                }

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var response = Task.FromResult(new ExportResultResponse {
                    FileName = "Orders.xlsx",
                    Data = package.GetAsByteArray()
                });

                return response;
            }
        }


        public async Task<bool> UpdateInvoiceOrder(OrderListDto orderDto)
        {
            var model = _orderRepository.FirstOrDefault(t => t.AccountNo == orderDto.AccountNo && t.CustomerFirstName == orderDto.CustomerFirstName);
            bool exist = false;
            if (model != null)
            {
                model.PaymentStatusId = (int)PaymentStatusEnum.Done;
                await _orderRepository.UpdateAsync(model);
                exist = true;
            }
            return exist;
        }


        public List<OrderListDto> ReadInvoiceFile(IFormFile formFile)
        {
            List<OrderListDto> orderDtoList = new List<OrderListDto>();

            int row = 0;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            int cont = 1;
            int customerFirstName = 0;
            int accountNo = 0;
            
            using (var reader = ExcelReaderFactory.CreateReader(formFile.OpenReadStream()))
            {
                while (reader.Read()) //Each ROW
                {
                    if(row == 0)//read column numbers
                    {
                        for (int column = 0; column < reader.FieldCount; column++)
                        {
                            if (getValue(reader, column) == L("CustomerName")) customerFirstName = column;
                            if (getValue(reader, column) == L("Account")) accountNo = column;
                        }
                    }
                    else
                    {
                        OrderListDto orderDto = new OrderListDto();
                        for (int column = 0; column < reader.FieldCount; column++)
                        {
                            if (column == customerFirstName) orderDto.CustomerFirstName = getValue(reader, column); 
                            if (column == accountNo) orderDto.AccountNo = getValue(reader, column);
                        }
                        if(orderDto.AccountNo != null)
                        {
                            orderDto.Id = cont++;
                            orderDtoList.Add(orderDto);
                        }
                    }
                    row++;
                }
            }

            foreach (var item in orderDtoList)
            {
                if (UpdateInvoiceOrder(item).Result)
                    item.Notes = L("Loaded");
                else
                    item.Notes = L("NoFounded");
            }
            return orderDtoList;
        }
        public static string getValue(IExcelDataReader reader, int column)
        {
            string val = null;
            if (reader.GetValue(column) != null)
            {
                val = reader.GetValue(column).ToString();
            }
            return val;
        }
    }
}
