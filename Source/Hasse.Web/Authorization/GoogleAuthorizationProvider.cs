using Hasse.Web.Models;

namespace Hasse.Web.Authorization
{
    class GoogleAuthorizationProvider : OAuth2AuthorizationProvider
    {
        public GoogleAuthorizationProvider()
            : base("google", "https://accounts.google.com/o/oauth2/auth", "https://accounts.google.com/o/oauth2/token", "386554152355.apps.googleusercontent.com", "2rQ82bH5jWmh0BIAdYuN1Wbh")
        {

        }

        public override string[] Scope { get { return new[] { "https://www.googleapis.com/auth/userinfo.profile", "https://www.googleapis.com/auth/userinfo.email" }; } }

        public override AuthModel GetAuthInfo(string accessToken)
        {
            var result = SignedCall("https://www.googleapis.com/oauth2/v1/userinfo", accessToken);

            return new AuthModel() {
                AccessToken = accessToken,
                Id = result.Value<string>("id"),
                Username = null,
                Name = result.Value<string>("name"),
                Email = result.Value<string>("email")
            };
        }
    }
}