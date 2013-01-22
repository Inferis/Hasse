using System.Linq;
using System.Web.Mvc;
using Hasse.Models;
using Hasse.Web.Models.Main;
using Raven.Client.Linq;

namespace Hasse.Web.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            // we need all posts
            //var stories = this.RavenSession().Query<Story>().Where(x => x.Visible).ToList();

            return View(new IndexViewModel());
        }

    }
}
