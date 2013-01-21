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
        public static readonly string[] ScopeNeeded = { "email", "publish_stream" };

        private WebServerClient FacebookClient()
        {
            var descr = new AuthorizationServerDescription {
                TokenEndpoint = new Uri("https://graph.facebook.com/oauth/access_token"),
                AuthorizationEndpoint = new Uri("https://graph.facebook.com/oauth/authorize")
            };

            var client = new WebServerClient(descr) {
                ClientIdentifier = "335994424835",
                ClientCredentialApplicator = ClientCredentialApplicator.PostParameter("01832afd7614f05840b908322377fe63"),
            };

            return client;
        }

        public ActionResult Index()
        {
            var client = FacebookClient();
            var uri = new Uri(Request.Url.Scheme + "://" + Request.Url.Host + Url.Action("OAuthed", new { id = "facebook" }));
            return client.PrepareRequestUserAuthorization(ScopeNeeded, uri).AsActionResult();
        }



        public ActionResult OAuthed(string id)
        {
            if (id == "facebook") {
                var client = FacebookClient();
                var state = client.ProcessUserAuthorization();
                if (state != null) {
                    var graph = new Facebook.FacebookClient(state.AccessToken);
                    var me = graph.Get<dynamic>("me");
                    return View(new AuthModel { AccessToken = state.AccessToken, Name = me.name, Email = me.email });
                }
            }

            return View();
        }
    }


}