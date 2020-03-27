using System.Collections.Generic;
using ServiceControl.Roles.Dto;

namespace ServiceControl.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
