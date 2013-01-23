using System.Linq;
using System.Web.Mvc;
using Hasse.Common;
using Hasse.Models;
using Hasse.Web.Extensions;
using Hasse.Web.Models.Main;
using Raven.Client.Linq;

namespace Hasse.Web.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Index(int? page)
        {
            var isAdmin = User.IsInRole(HasseRoles.Admin);

            // we need all posts
            var stories = this.RavenSession().Query<Story>().OrderByDescending(x => x.PostDate);
            // admins can see everything, the rest can't
            if (!isAdmin)
                stories = stories.Where(x => x.State == StoryState.Published);

            var model = new IndexViewModel() {
                CanAddStory = isAdmin,
                Stories = stories.Skip(page.GetValueOrDefault(0) * 20).Take(20).ToList()
            };

            return View(model);
        }

    }
}
