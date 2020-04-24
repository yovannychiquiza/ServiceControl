using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ServiceControl.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceControl.Orders.Dto
{
    [AutoMapTo(typeof(Orders))]
    public class OrderDto : EntityDto<long>
    {

        public int CompanyId { get; set; }

        [MaxLength(Orders.MaxDescriptionLength)]
        public string Serial { get; set; }

        public DateTime DateBooked { get; set; }
        public string Sgi { get; set; }
        public long SalesRepId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string ContactPhone { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public int FirstIdentificationId { get; set; }
        public int SecondIdentificationId { get; set; }
        public string ExistingAccountNo { get; set; }
        public string StreetNo { get; set; }
        public string CustomerAddress { get; set; }
        public string Unit { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PromoDetails { get; set; }
        public string Notes { get; set; }
        public DateTime CreationDate { get; set; }
        public int OrderStateId { get; set; }
        public int TimeSlotId { get; set; }
        public OrderState OrderState { get; set; }
        public string OrderStateName { get; set; }
        public Company Company { get; set; }
        public User SalesRep { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public FirstIdentification FirstIdentification { get; set; }
        public SecondIdentification SecondIdentification { get; set; }
        public string OrderNo { get; set; }
        public string AccountNo { get; set; }
        public string InstallDate { get; set; }
        public string Remarks { get; set; }
        public string Followed { get; set; }
        public string Explanation { get; set; }
        public string IsReady { get; set; }
        public long? SubSalesRepId { get; set; }
    }
}
