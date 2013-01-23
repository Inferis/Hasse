using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Hasse.Common;
using Hasse.Models;
using Hasse.Web.Authorization;
using Hasse.Web.Extensions;
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
                ExternalAuthenticationInfo externalInfo = null;
                if (accessToken != null && !string.IsNullOrEmpty(accessToken.Item1)) {
                    externalInfo = provider.GetAuthenticationInfo(accessToken.Item1);
                }

                if (externalInfo == null) {
                    // todo error report to user
                    return RedirectToAction("SignIn");
                }
                externalInfo.ProviderId = id;

                // lookup user
                var user = this.RavenSession()
                    .Query<User>()
                    .FirstOrDefault(u => u.ExternalReferences.Any(r => r.ProviderId == id && r.Reference == externalInfo.Id));

                if (user != null) {
                    // found
                    user.IntegrateExternalService(externalInfo);
                    this.RavenSession().Store(user);
                    FormsAuthentication.SetAuthCookie(user.Id, true);

                    return RedirectToAction("Index", "Main");
                }

                // create new
                TempData["ExternalAuthenticationInfo"] = externalInfo;
                return RedirectToAction("Register");
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
            if (string.IsNullOrEmpty(id)) {
                if (!User.Identity.IsAuthenticated)
                    return RedirectToAction("SignIn");
                id = User.Identity.Name;
            }

            return Profile(id);
        }

        public ActionResult Register()
        {
            var info = TempData["ExternalAuthenticationInfo"] as ExternalAuthenticationInfo;

            RegisterViewModel model;
            if (info != null) {
                TempData["ExternalAuthenticationInfo"] = info;
                model = new RegisterViewModel() {
                    Email = info.Email,
                    Name = info.Name,
                    ProviderId = info.ProviderId,
                    ProviderReference = info.Id,
                    TwitterScreenname = info.ProviderId == "twitter" ? info.Username : null,
                    FacebookUsername = info.ProviderId == "facebook" ? info.Username : null,
                };
            }
            else {
                model = new RegisterViewModel();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // create new user
            var user = new User() {
                Email = model.Email,
                Name = model.Name,
                TwitterName = model.TwitterScreenname,
                FacebookUserName = model.FacebookUsername
            };
            user.SetPassword(model.Password);
            user.ExternalReferences.Add(new UserReference() { ProviderId = model.ProviderId, Reference = model.ProviderReference });

            // save user
            this.RavenSession().Store(user);
            this.RavenSession().SaveChanges();
            FormsAuthentication.SetAuthCookie(user.Id, true);

            this.AddInformation("Je registratie is voltooid.");
            this.AddCallToAction("Je kan nu je profiel aanvullen, als je dat wilt. Je kan dit ook later doen.", Url.Action("Index", new { id = user.Id }));

            return RedirectToAction("Index", "Main");
        }

        private ActionResult Profile(string id)
        {
            var user = this.RavenSession().Load<User>(id);

            ProfileViewModel model;
            if (user == null) {
                model = new ProfileViewModel() { Found = false, Id = id };
            }
            else {
                model = new ProfileViewModel() {
                    Found = true,
                    Id = id,
                    Name = user.Name,
                    FacebookUserName = user.FacebookUserName,
                    TwitterUserName = user.TwitterName,
                };
                if (User.IsInRole(HasseRoles.Admin) || user.Id == User.Identity.Name)
                    model.Email = user.Email;
            }
            return View("Profile", model);
        }
    }
}