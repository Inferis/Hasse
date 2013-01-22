using System.Web;
using System.Web.Mvc;

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