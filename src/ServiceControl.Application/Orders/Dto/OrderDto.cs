﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
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
        public string SalesRep { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string ContactPhone { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PrimaryId { get; set; }
        public string SecondaryId { get; set; }
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
        public OrderState OrderState { get; set; }
        public Company Company { get; set; }

    }
}
