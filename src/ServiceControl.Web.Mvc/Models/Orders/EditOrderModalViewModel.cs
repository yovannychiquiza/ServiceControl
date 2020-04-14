using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceControl.Orders.Dto;
using System.Collections.Generic;

namespace ServiceControl.Web.Models.Orders
{
    public class EditOrderModalViewModel
    {
        public OrderDto Order { get; set; }
        public List<SelectListItem> OrderState { get; set; }
        public List<SelectListItem> Company { get; set; }
        public List<SelectListItem> FirstIdentification { get; set; }
        public List<SelectListItem> SecondIdentification { get; set; }
        public List<SelectListItem> TimeSlot { get; set; }
        public List<SelectListItem> Followed { get; set; }
        
    }
}
