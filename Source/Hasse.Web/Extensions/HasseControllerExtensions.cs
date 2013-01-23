using System.Web.Mvc;
using Hasse.Models;
using Raven.Client;

namespace Hasse.Web.Extensions
{
    public static class HasseControllerExtensions
    {
        public static User AuthenticatedUser(this Controller controller)
        {
            return controller.User.Identity.IsAuthenticated 
                ? controller.RavenSession().Load<User>(controller.User.Identity.Name) 
                : null;
        }

        public static IDocumentSession RavenSession(this ControllerBase controller)
        {
            return controller.TempData["RavenSession"] as IDocumentSession;
        }

        public static void AddCallToAction(this ControllerBase controller, string message, string link)
        {
        }

        public static void AddInformation(this ControllerBase controller, string message)
        {
        }

        public static void AddError(this ControllerBase controller, string message)
        {
        }
    }
}