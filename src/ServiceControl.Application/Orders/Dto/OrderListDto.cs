﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using ServiceControl.Authorization.Users;
using ServiceControl.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceControl.Orders.Dto
{
    [AutoMapFrom(typeof(Orders))]
    public class OrderListDto : EntityDto
    {
        public int CompanyId { get; set; }

        public string Serial { get; set; }

        public DateTime DateBooked { get; set; }

        public string Sgi { get; set; }
        public long SalesRepId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string ContactPhone { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
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
        public int TimeSlotId { get; set; }
        public OrderState OrderState { get; set; }
        public Company Company { get; set; }
        public UserDto SalesRep { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public FirstIdentification FirstIdentification { get; set; }
        public SecondIdentification SecondIdentification { get; set; }


    }
}
