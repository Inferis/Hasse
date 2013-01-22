using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using Hasse.Web.Models;
using Newtonsoft.Json.Linq;

namespace Hasse.Web.Authorization
{
    abstract class OAuth2AuthorizationProvider : IAuthorizationProvider
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly AuthorizationServerDescription description;

        protected OAuth2AuthorizationProvider(string id, string authorizationEndpoint, string tokenEndpoint, string clientId, string clientSecret)
        {
            Id = id;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            description = new AuthorizationServerDescription {
                TokenEndpoint = new Uri(tokenEndpoint),
                AuthorizationEndpoint = new Uri(authorizationEndpoint),
                ProtocolVersion = ProtocolVersion.V20
            };
        }

        protected virtual WebServerClient GetClient()
        {
            var client = new WebServerClient(description) {
                ClientIdentifier = clientId,
                ClientCredentialApplicator = ClientCredentialApplicator.PostParameter(clientSecret),
            };

            return client;
        }

        public ActionResult StartAuthorization(Func<string, string> callbackGenerator)
        {
            return GetClient()
                .PrepareRequestUserAuthorization(Scope, new Uri(callbackGenerator(Id)))
                .AsActionResult();
        }

        public Tuple<string, DateTime> FinishAuthorization()
        {
            var state = GetClient().ProcessUserAuthorization();
            if (state != null && !string.IsNullOrEmpty(state.AccessToken)) {
                return new Tuple<string, DateTime>(state.AccessToken, state.AccessTokenExpirationUtc.GetValueOrDefault(DateTime.Now));
            }
            return null;
        }

        public abstract AuthModel GetAuthInfo(string accessToken);

        public string Id { get; private set; }
        public abstract string[] Scope { get; }

        protected JObject SignedCall(string uri, string accessToken, string accessTokenName = "access_token")
        {
            uri = string.Concat(uri, uri.Contains("?") ? "&" : "?", HttpUtility.UrlEncode(accessTokenName), "=", HttpUtility.UrlEncode(accessToken));

            using (var client = new WebClient())
            using (var stream = client.OpenRead(uri))
            using (var reader = new StreamReader(stream))
                return JObject.Parse(reader.ReadToEnd());
        }

    }
}