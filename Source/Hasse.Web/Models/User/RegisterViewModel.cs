using Hasse.Web.Extensions;
using Hasse.Web.Extensions.Attributes;

namespace Hasse.Web.Models.User
{
    public class RegisterViewModel
    {
        [LocalizedDisplayName("RegisterViewModel", "Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("RegisterViewModel", "Email")]
        public string Email { get; set; }

        [LocalizedDisplayName("RegisterViewModel", "Password")]
        public string Password { get; set; }

        [LocalizedDisplayName("RegisterViewModel", "ConfirmPassword")]
        public string ConfirmPassword { get; set; }

        [LocalizedDisplayName("RegisterViewModel", "Website")]
        public string Website { get; set; }

        public string ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderReference { get; set; }
        public string TwitterScreenname { get; set; }
        public string FacebookUsername { get; set; }
    }
}