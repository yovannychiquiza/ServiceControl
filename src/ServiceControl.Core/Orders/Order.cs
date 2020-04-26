using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ServiceControl.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServiceControl.Orders
{
    [Table("Orders")]
    public class Orders : Entity
    {
        public Orders()
        {
            OrdersProductType = new HashSet<OrdersProductType>();
        }
        public const int MaxTitleLength = 256;
        public const int MaxDescriptionLength = 64 * 1024; //64KB

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        [MaxLength(MaxDescriptionLength)]
        public string Serial { get; set; }
        public DateTime DateBooked { get; set; }
        public string Sgi { get; set; }
        public long SalesRepId { get; set; }
        [Required]
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
        public int OrderStateId { get; set; }
        [ForeignKey(nameof(OrderStateId))]
        public OrderState OrderState { get; set; }

        [ForeignKey(nameof(SalesRepId))]
        public User SalesRep { get; set; }
        public int TimeSlotId { get; set; }
        [ForeignKey(nameof(TimeSlotId))]
        public TimeSlot TimeSlot { get; set; }
        [ForeignKey(nameof(SecondIdentificationId))]
        public SecondIdentification SecondIdentification { get; set; }
        [ForeignKey(nameof(FirstIdentificationId))]
        public FirstIdentification FirstIdentification { get; set; }
        public string OrderNo { get; set; }
        public string AccountNo { get; set; }
        public DateTime? InstallDate { get; set; }
        public string Remarks { get; set; }
        public string Followed { get; set; }
        public string Explanation { get; set; }
        public Boolean IsReady { get; set; }

        public virtual ICollection<OrdersProductType> OrdersProductType { get; set; }

    }
}
