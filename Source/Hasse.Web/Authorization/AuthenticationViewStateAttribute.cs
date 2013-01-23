using System.Web.Mvc;
using Hasse.Web.Extensions;

namespace Hasse.Web.Authorization
{
    public class AuthenticationViewStateAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsAuthenticated = false;

            var controller = filterContext.Controller as Controller;
            if (controller != null) {
                var user = controller.AuthenticatedUser();
                if (user != null) {
                    controller.ViewBag.IsAuthenticated = true;
                    controller.ViewBag.AuthenticatedUserId = user.Id;
                    controller.ViewBag.AuthenticatedUserName = user.Name;
                    controller.ViewBag.AuthenticatedUserEmail = user.Email;
                }
            }
            base.OnResultExecuting(filterContext);
        }
    }
}