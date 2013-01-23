using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Hasse.Models;
using Hasse.Web.Extensions.Attributes;

namespace Hasse.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RavenSessionAttribute());
        }
    }
}