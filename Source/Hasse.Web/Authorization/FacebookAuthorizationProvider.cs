using Hasse.Web.Models;

namespace Hasse.Web.Authorization
{
    class FacebookAuthorizationProvider : OAuth2AuthorizationProvider
    {
        public FacebookAuthorizationProvider()
            : base("facebook", "https://graph.facebook.com/oauth/authorize", "https://graph.facebook.com/oauth/access_token", "335994424835", "01832afd7614f05840b908322377fe63")
        {

        }

        public override string[] Scope { get { return new[] { "email", "publish_stream" }; } }

        public override AuthModel GetAuthInfo(string accessToken)
        {
            var graph = new Facebook.FacebookClient(accessToken);
            var me = graph.Get<dynamic>("me");
            return new AuthModel {
                AccessToken = accessToken,
                Id = me.id,
                Username = me.username,
                Name = me.name,
                Email = me.email
            };
        }
    }
}