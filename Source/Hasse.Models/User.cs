using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Hasse.Common;
using Hasse.Common.Extensions;

namespace Hasse.Models
{
    public class User : Identifiable
    {
        public User()
        {
            ExternalReferences = new List<UserReference>();
        }

        public string Name { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        private string Password { get; set; }
        public string TwitterName { get; set; }
        public string FacebookUserName { get; set; }

        public List<UserReference> ExternalReferences { get; private set; }

        public void IntegrateExternalService(ExternalAuthenticationInfo info)
        {
            if (info == null)
                return;

            var current = ExternalReferences.FirstOrDefault(e => e.ProviderId == info.ProviderId);
            if (current != null) {
                current.AccessToken = info.AccessToken;
                current.AccessTokenExpires = info.AccessTokenExpiration;
            }
            else {
                current = new UserReference() {
                    ProviderId = info.ProviderId,
                    Reference = info.Id,
                    AccessToken = info.AccessToken,
                    AccessTokenExpires = info.AccessTokenExpiration,
                };
                ExternalReferences.Add(current);
            }

            if (info.ProviderId == "twitter" && string.IsNullOrEmpty(TwitterName)) {
                TwitterName = info.Username;
            }
            if (info.ProviderId == "facebook" && string.IsNullOrEmpty(TwitterName)) {
                FacebookUserName = info.Username;
            }
        }

        public void SetPassword(string password)
        {
            Password = string.IsNullOrEmpty(password) ? null : password.MD5();
        }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            return Password == password.MD5();
        }
    }
}
