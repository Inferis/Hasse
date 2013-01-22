using System;
using System.Linq;
using System.Web.Mvc;
using Hasse.Models;
using Hasse.Web.Authorization;
using Hasse.Web.Models;
using Hasse.Web.Models.User;
using Hasse.Web.Resources;

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

        #region Signin
        public ActionResult SignIn(string id)
        {
            var provider = GetProvider(id);
            if (provider == null) {
                var model = new SigninViewModel() {
                    Providers = Providers.Select(x => new ProviderViewModel() {
                        Id = x.Id,
                        Name = Translations.ResourceManager.GetString("provider_" + x.Id + "_name", Translations.Culture)
                    }).ToList()
                };
                return View(model);
            }

            return provider.StartAuthorization(x => Request.Url.Scheme + "://" + Request.Url.Host + Url.Action("OAuthed", new { id = x }));
        }

        public ActionResult OAuthed(string id)
        {
            var provider = GetProvider(id);
            if (provider == null) {
                // todo error report to user
                return RedirectToAction("SignIn");
            }

            try {
                var accessToken = provider.FinishAuthorization();
                AuthModel model = null;
                if (accessToken != null && !string.IsNullOrEmpty(accessToken.Item1)) {
                    model = provider.GetAuthInfo(accessToken.Item1);
                }

                if (model == null) {
                    // todo error report to user
                    return RedirectToAction("SignIn");
                }
                model.ProviderId = id;

                // lookup user
                var user = this.RavenSession()
                    .Query<User>()
                    .FirstOrDefault(u => u.ExternalReferences.Any(r => r.ProviderId == id && r.Reference == model.Id));

                if (user != null) {
                    // found
                    return RedirectToAction("Signin");
                }
                else {
                    // create new
                    TempData["ExternalReference"] = model;
                    return RedirectToAction("Register");
                }
            }
            catch (Exception) {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SignIn(SigninViewModel model)
        {
            return HttpNotFound();
        }
        #endregion



        public ActionResult Index(string id)
        {
            var provider = GetProvider(id);
            if (provider == null)
                return View(Providers);

            return provider.StartAuthorization(x => Request.Url.Scheme + "://" + Request.Url.Host + Url.Action("OAuthed", new { id = x }));
        }

        public ActionResult Info()
        {
            var model = TempData["AuthModel"] as AuthModel;
            if (model == null) {
                return HttpNotFound();
            }

            return View(model);
        }

        public ActionResult Register()
        {
            throw new NotImplementedException();
        }
    }


}