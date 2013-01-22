using System.Collections.Generic;

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
        public string Password { get; set; }

        public List<UserReference> ExternalReferences { get; private set; }
    }

    public class UserReference
    {
        public string ProviderId { get; set; }
        public string Reference { get; set; }
    }
}
