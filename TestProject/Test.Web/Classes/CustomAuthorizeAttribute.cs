using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Test.Web.Classes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public CustomAuthorizeAttribute(string roles)
        {
            _roles = roles.Split(',').Select(r => r.Trim()).ToArray();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userRole = context.HttpContext.Session.GetInt32("Role");
            if (userRole == null || userRole == 0 || !_roles.Contains(((RoleEnum)userRole).ToString()))
            {
                context.Result = new RedirectToActionResult("unauthorized", "account", null);
            }
        }
    }
}
