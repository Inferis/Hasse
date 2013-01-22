using System;
using System.IO;
using System.Net;
using DotNetOpenAuth.OAuth2;
using Hasse.Web.Models;
using Newtonsoft.Json.Linq;

namespace Hasse.Web.Controllers
{
    class GoogleAuthorizationProvider : IAuthorizationProvider
    {
        public WebServerClient GetOAuthClient()
        {
            var descr = new AuthorizationServerDescription {
                AuthorizationEndpoint = new Uri("https://accounts.google.com/o/oauth2/auth"),
                TokenEndpoint = new Uri("https://accounts.google.com/o/oauth2/token"),
                ProtocolVersion = ProtocolVersion.V20
            };

            var client = new WebServerClient(descr) {
                ClientIdentifier = "386554152355.apps.googleusercontent.com",
                ClientCredentialApplicator = ClientCredentialApplicator.PostParameter("2rQ82bH5jWmh0BIAdYuN1Wbh")
            };

            return client;
        }

        public string Id { get { return "google"; } }
        public string[] Scope { get { return new[] { "https://www.googleapis.com/auth/userinfo.profile", "https://www.googleapis.com/auth/userinfo.email" }; } }

        public AuthModel GetAuthInfo(string accessToken)
        {
            var client = new WebClient();
            JObject result;
            using (var stream = client.OpenRead("https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + accessToken)) {
                using (var reader = new StreamReader(stream)) {
                    result = JObject.Parse(reader.ReadToEnd());
                }
            }

            return new AuthModel() {
                                       AccessToken = accessToken,
                                       Name = result["name"].Value<string>(),
                                       Email = result["email"].Value<string>()
                                   };
        }
    }
}