using System.Web.Mvc;
using Raven.Client;

namespace Hasse.Web.Extensions.Attributes
{
    public class RavenSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction) {
                filterContext.Controller.TempData["RavenSession"] = MvcApplication.Store.OpenSession();
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (filterContext.IsChildAction) 
                return;
            
            using (var session = filterContext.Controller.TempData["RavenSession"] as IDocumentSession) {
                if (filterContext.Exception != null)
                    return;

                if (session != null)
                    session.SaveChanges();
            }
        }

    }
}