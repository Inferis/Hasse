using System;
using DotNetOpenAuth.OAuth2;
using Hasse.Web.Models;

namespace Hasse.Web.Controllers
{
    class FacebookAuthorizationProvider : IAuthorizationProvider
    {
        public WebServerClient GetOAuthClient()
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

        public string Id { get { return "facebook"; } }
        public string[] Scope { get { return new[] { "email", "publish_stream" }; } }

        public AuthModel GetAuthInfo(string accessToken)
        {
            var graph = new Facebook.FacebookClient(accessToken);
            var me = graph.Get<dynamic>("me");
            return new AuthModel { AccessToken = accessToken, Name = me.name, Email = me.email };
        }
    }
}