using Newtonsoft.Json;

namespace Hasse.Models
{
    [JsonObject(IsReference = true)]
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
