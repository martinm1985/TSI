using Crud.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Crud.Permissions
{
    public class AdminAuthorization : AuthorizationHandler<AdminRequirements>
    {
        UserManager<User> _userManager;
        public AdminAuthorization(UserManager<User> UserManager)
        {
            _userManager = UserManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, AdminRequirements requirement
        )
        {
            var user = await _userManager.GetUserAsync(context.User);

            if (user != null && user.isAdministrador)
            {
                context.Succeed(requirement);
            }

            return;
        }
    }
}
