using System.Collections.Generic;
using ServiceControl.Roles.Dto;

namespace ServiceControl.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
