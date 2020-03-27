using System.Collections.Generic;
using ServiceControl.Roles.Dto;

namespace ServiceControl.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}