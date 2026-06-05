using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Isp.Web.Controllers
{
    public class BaseController : Controller
    {
        protected int? _currentUserId => HttpContext.Session.GetInt32("UserId");
        protected int? _currentRole => HttpContext.Session.GetInt32("Role");
        protected string _currentFullName => HttpContext.Session.GetString("FullName");

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if user session exists
            if (!_currentUserId.HasValue)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
