﻿using Hasse.Common;

namespace Hasse.Web.Authorization
{
    class GithubAuthorizationProvider : OAuth2AuthorizationProvider {
        public GithubAuthorizationProvider() : base("github", "https://github.com/login/oauth/authorize", "https://github.com/login/oauth/access_token", "decff741693c909909cc", "faabebc90edec3a85bafeaa88a565e4deafe3d46") { }

        public override ExternalAuthenticationInfo GetAuthenticationInfo(string accessToken)
        {
            var result = SignedCall("https://api.github.com/user", accessToken);

            return new ExternalAuthenticationInfo() {
                                       AccessToken = accessToken,
                                       Id = result.Value<string>("id"),
                                       Username = result.Value<string>("login"),
                                       Name = result.Value<string>("name"),
                                       Email = result.Value<string>("email")
                                   };
        }

        public override string[] Scope
        {
            get { return new [] { "user:email" }; }
        }
    }
}