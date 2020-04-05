using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ServiceControl.Authorization.Users;
using ServiceControl.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServiceControl.UserCompany
{
    [Table("SalesRep_Company")]
    public class SalesRepCompany : Entity
    {

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public int CompanyId { get; set; }

        public long SalesRepId { get; set; }

        [ForeignKey(nameof(SalesRepId))]
        public User SalesRep { get; set; }

        public string Code { get; set; }

    }
}
