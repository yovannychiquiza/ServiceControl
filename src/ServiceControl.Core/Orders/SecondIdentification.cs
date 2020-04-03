using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ServiceControl.Orders
{
    [Table("SecondIdentification")]
    public class SecondIdentification : Entity
    {
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }

    }
}
