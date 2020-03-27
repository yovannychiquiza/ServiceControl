using Abp.Authorization;
using ServiceControl.Authorization.Roles;
using ServiceControl.Authorization.Users;

namespace ServiceControl.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
