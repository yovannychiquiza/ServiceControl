using ServiceControl.Orders.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceControl.Web.Models.Orders
{
    public class IndexViewModel
    {
        public List<OrderListDto> Orders { get; }

        public string SelectedTaskState { get; set; }

        public IndexViewModel(List<OrderListDto> orders)
        {
            Orders = orders;
        }

    }
}
