using System.Collections.Generic;

namespace Hasse.Models
{
    public class StoryProtection
    {
        public StoryProtection()
        {
            ReleasedFor = new List<User>();
        }

        public string Password { get; set; }
        public List<User> ReleasedFor { get; private set; }
    }
}