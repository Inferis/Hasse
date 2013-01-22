using System.Linq;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using Hasse.Web.Models;

namespace Hasse.Web.Controllers
{
    class YahooAuthorizationProvider : OAuth1AuthorizationProvider {
        public YahooAuthorizationProvider() : base("yahoo", "dj0yJmk9M3dHQzltYVl3YW8zJmQ9WVdrOU9YRnhSMWRJTjJzbWNHbzlNalUwTVRjNE5qSS0mcz1jb25zdW1lcnNlY3JldCZ4PTdm", "604cce34cec7e069ed3a0a4b70aa62bf0e1b9c35") { }

        protected override ServiceProviderDescription SignInServiceDescription
        {
            get
            {
                return new ServiceProviderDescription {
                    RequestTokenEndpoint = new MessageReceivingEndpoint("https://api.login.yahoo.com/oauth/v2/get_request_token", HttpDeliveryMethods.PostRequest),
                    UserAuthorizationEndpoint = new MessageReceivingEndpoint("https://api.login.yahoo.com/oauth/v2/request_auth", HttpDeliveryMethods.GetRequest),
                    AccessTokenEndpoint = new MessageReceivingEndpoint("https://api.login.yahoo.com/oauth/v2/get_token", HttpDeliveryMethods.GetRequest),
                    TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
                };
            }
        }

        public override AuthModel GetAuthInfo(string accessToken)
        {
            var me = SignedCall("http://social.yahooapis.com/v1/me/guid?format=json", accessToken);
            var guid = me["guid"].Value<string>("value");

            var profile = SignedCall("http://social.yahooapis.com/v1/user/" + guid + "/profile?format=json", accessToken);
            var result = profile["profile"];

            return new AuthModel() {
                AccessToken = accessToken,
                Id = result.Value<string>("guid"),
                Username = result.Value<string>("nickname"),
                Name = string.Concat(result.Value<string>("givenName"), " ", result.Value<string>("familyName")),
                Email = result["emails"].Children().Where(x => x.Value<bool>("primary")).Select(x => x.Value<string>("handle")).FirstOrDefault()
            };
        }
    }
}