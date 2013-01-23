using System;

namespace Hasse.Common
{
    public class ExternalAuthenticationInfo
    {
        public string ProviderId { get; set; }
        public string Id { get; set; }
        public string AccessToken { get; set; }
        public DateTimeOffset AccessTokenExpiration { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}