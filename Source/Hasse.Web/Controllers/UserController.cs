using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using Hasse.Web.Models;

namespace Hasse.Web.Controllers
{
    public class UserController : Controller
    {
        public static readonly IAuthorizationProvider[] Providers = { new FacebookAuthorizationProvider(), new GoogleAuthorizationProvider() };
        private IAuthorizationProvider GetProvider(string id)
        {
            return Providers.FirstOrDefault(x => x.Id == id);
        }

        public ActionResult Index(string id)
        {
            var provider = GetProvider(id);
            if (provider == null)
                return View(Providers);

            var uri = new Uri(Request.Url.Scheme + "://" + Request.Url.Host + Url.Action("OAuthed", new { id = provider.Id }));
            return provider.GetOAuthClient()
                .PrepareRequestUserAuthorization(provider.Scope, uri)
                .AsActionResult();
        }

        public ActionResult OAuthed(string id)
        {
            var provider = GetProvider(id);
            if (provider == null)
                return HttpNotFound();

            var state = provider.GetOAuthClient().ProcessUserAuthorization();
            AuthModel model = null;
            if (state != null && !string.IsNullOrEmpty(state.AccessToken)) {
                // todo register access token
                model = provider.GetAuthInfo(state.AccessToken);
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