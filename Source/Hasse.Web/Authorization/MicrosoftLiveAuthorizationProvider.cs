using Hasse.Web.Models;
using Newtonsoft.Json.Linq;

namespace Hasse.Web.Authorization
{
    class MicrosoftLiveAuthorizationProvider : OAuth2AuthorizationProvider {
        public MicrosoftLiveAuthorizationProvider()
            : base("liveconnect", "https://login.live.com/oauth20_authorize.srf", "https://login.live.com/oauth20_token.srf", "000000004C0E066C", "FVwIGMZPVO0Al7q7ppWmwQXaCwrFToRk")
        {

        }

        public override string[] Scope { get { return new string[] { "wl.emails", "wl.basic" }; } }

        public override AuthModel GetAuthInfo(string accessToken)
        {
            var result = SignedCall("https://apis.live.net/v5.0/me", accessToken);

            return new AuthModel() {
                AccessToken = accessToken,
                Id = result["id"].Value<string>(),
                Username = null,
                Name = result["name"].Value<string>(),
                Email = result["emails"]["preferred"].Value<string>()
            };
        }
    }
}