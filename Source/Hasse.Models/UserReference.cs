using System;

namespace Hasse.Models
{
    public class UserReference
    {
        public string ProviderId { get; set; }
        public string Reference { get; set; }
        public string AccessToken { get; set; }
        public DateTimeOffset AccessTokenExpires { get; set; }
    }
}