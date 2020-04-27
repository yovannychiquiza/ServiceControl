using System.Collections.Generic;
using System.Linq;
using ServiceControl.Authorization.Users;
using ServiceControl.SubUser.Dto;
using ServiceControl.Users.Dto;

namespace ServiceControl.Web.Models.Users
{
    public class SubSalesRepModalViewModel
    {
        public UserDto User { get; set; }

        public List<SubSalesRepDto> SubSalesRepDto { get; set; }
        public List<User> SalesRep { get; set; }


        public bool SubSalesRepIsInSalesRep(User user)
        {
            return SubSalesRepDto != null && SubSalesRepDto.Any(r => r.SubSalesRepr != null && r.SubSalesRepr.Id == user.Id);
        }
        
    }
}
