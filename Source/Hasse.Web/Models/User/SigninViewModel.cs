using System.Collections.Generic;

namespace Hasse.Web.Models.User
{
    public class SigninViewModel
    {
        public List<ProviderViewModel> Providers { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public bool Authenticated { get; set; }
    }
}