using System;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using Hasse.Web.Models;
using Newtonsoft.Json.Linq;

namespace Hasse.Web.Controllers
{
    abstract class OAuth1AuthorizationProvider : IAuthorizationProvider
    {
        private readonly string consumerKey;
        private readonly string consumerSecret;

        protected OAuth1AuthorizationProvider(string id, string consumerKey, string consumerSecret)
        {
            Id = id;
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
        }

        protected abstract ServiceProviderDescription SignInServiceDescription { get; }

        public ActionResult StartAuthorization(Func<string, string> callbackGenerator)
        {
            var consumer = GetConsumer();
            var req = consumer.PrepareRequestUserAuthorization(new Uri(callbackGenerator(Id)), null, null);
            return consumer.Channel.PrepareResponse(req).AsActionResult();
        }

        public string FinishAuthorization()
        {
            var response = GetConsumer().ProcessUserAuthorization();
            return response != null ? response.AccessToken : null;
        }

        public string Id { get; private set; }
        public abstract AuthModel GetAuthInfo(string accessToken);

        private WebConsumer GetConsumer()
        {
            return new WebConsumer(SignInServiceDescription, TokenManager);
        }

        private InMemoryTokenManager TokenManager
        {
            get
            {
                var store = HttpContext.Current.Session;
                var tokenManager = (InMemoryTokenManager)store[Id + "TokenManager"];
                if (tokenManager == null) {
                    tokenManager = new InMemoryTokenManager(consumerKey, consumerSecret);
                    store[Id + "TokenManager"] = tokenManager;
                }

                return tokenManager;
            }
        }

        protected JObject SignedCall(string uri, string accessToken)
        {
            using (var response = GetConsumer().PrepareAuthorizedRequestAndSend(new MessageReceivingEndpoint(uri, HttpDeliveryMethods.GetRequest), accessToken))
            using (var reader = response.GetResponseReader())
                return JObject.Parse(reader.ReadToEnd());
        }

    }
}