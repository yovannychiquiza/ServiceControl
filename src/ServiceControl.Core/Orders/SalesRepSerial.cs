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
    [Table("SalesRepSerial")]
    public class SalesRepSerial : Entity
    {
        public long SalesRepId { get; set; }
        [ForeignKey(nameof(SalesRepId))]
        public User SalesRep { get; set; }
        public int Serial { get; set; }
    }
}
