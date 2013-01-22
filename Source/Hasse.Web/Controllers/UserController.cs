using System;
using System.Linq;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using Hasse.Web.Models;

namespace Hasse.Web.Controllers
{
    public class UserController : Controller
    {
        public static readonly IAuthorizationProvider[] Providers = {
                                                                        new FacebookAuthorizationProvider(), 
                                                                        new GoogleAuthorizationProvider(),
                                                                        new MicrosoftLiveAuthorizationProvider(), 
                                                                        new YahooAuthorizationProvider(), 
                                                                        new TwitterAuthorizationProvider(), 
                                                                        new GithubAuthorizationProvider(), 
                                                                    };
        private IAuthorizationProvider GetProvider(string id)
        {
            return Providers.FirstOrDefault(x => x.Id == id);
        }

        public ActionResult Index(string id)
        {
            var provider = GetProvider(id);
            if (provider == null)
                return View(Providers);

            return provider.StartAuthorization(x => Request.Url.Scheme + "://" + Request.Url.Host + Url.Action("OAuthed", new { id = x }));
        }

        public ActionResult OAuthed(string id)
        {
            var provider = GetProvider(id);
            if (provider == null)
                return HttpNotFound();

            var accessToken = provider.FinishAuthorization();
            AuthModel model = null;
            if (!string.IsNullOrEmpty(accessToken)) {
                // todo register access token
                model = provider.GetAuthInfo(accessToken);
            }

            if (model == null)
                return HttpNotFound();

            TempData["AuthModel"] = model;
            return RedirectToAction("Info");
        }

        public ActionResult Info()
        {
            var model = TempData["AuthModel"] as AuthModel;
            if (model == null) {
                return HttpNotFound();
            }

            return View(model);
        }
    }


}