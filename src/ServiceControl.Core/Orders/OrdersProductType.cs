using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServiceControl.Orders
{
    [Table("Orders_ProductType")]
    public class OrdersProductType : Entity
    {
        public int OrdersId { get; set; }
        [ForeignKey(nameof(OrdersId))]
        public virtual Orders Orders { get; set; }
        public int ProductTypeId { get; set; }
        [ForeignKey(nameof(ProductTypeId))]
        public virtual ProductType ProductType { get; set; }
    }
}
