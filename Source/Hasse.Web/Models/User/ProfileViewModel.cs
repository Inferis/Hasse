using System.Collections.Generic;

namespace Hasse.Web.Models.User
{
    public class ProfileViewModel
    {
        public bool Found { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string FacebookUserName { get; set; }
        public string TwitterUserName { get; set; }
        public List<Connection> Connections { get; set; }

        public class Connection {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }

}