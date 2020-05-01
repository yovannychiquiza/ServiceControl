using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServiceControl.Orders
{
    [Table("PaymentStatus")]
    public class PaymentStatus : Entity
    {
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }

    }
}
