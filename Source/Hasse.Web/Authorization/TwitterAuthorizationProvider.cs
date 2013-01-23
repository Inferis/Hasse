using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using Hasse.Common;
using Hasse.Models;
using Hasse.Web.Models;

namespace Hasse.Web.Authorization
{
    class TwitterAuthorizationProvider : OAuth1AuthorizationProvider
    {
        public TwitterAuthorizationProvider()
            : base("twitter", "7OgeLD3v5UjFlmfC4qsjA", "5Pb455wUpebbR42CR5spZQW79dokCsC4zeL23U9Muw")
        {

        }

        protected override ServiceProviderDescription SignInServiceDescription
        {
            get
            {
                return new ServiceProviderDescription {
                    RequestTokenEndpoint = new MessageReceivingEndpoint("http://twitter.com/oauth/request_token", HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
                    UserAuthorizationEndpoint = new MessageReceivingEndpoint("http://twitter.com/oauth/authenticate", HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
                    AccessTokenEndpoint = new MessageReceivingEndpoint("http://twitter.com/oauth/access_token", HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest),
                    TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
                };
            }
        }

        public override ExternalAuthenticationInfo GetAuthenticationInfo(string accessToken)
        {
            var result = SignedCall("https://api.twitter.com/1.1/account/verify_credentials.json", accessToken);

            return new ExternalAuthenticationInfo() {
                AccessToken = accessToken,
                Id = result.Value<string>("id"),
                Username = result.Value<string>("screen_name"),
                Name = result.Value<string>("name"),
                Email = null
            };
        }
    }
}