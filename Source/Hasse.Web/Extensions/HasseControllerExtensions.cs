using System.Web.Mvc;
using Raven.Client;

namespace Hasse.Web.Extensions
{
    public static class HasseControllerExtensions
    {
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