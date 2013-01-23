using System.Collections.Generic;
using Hasse.Models;

namespace Hasse.Web.Models.Main
{
    public class IndexViewModel
    {
        public bool CanAddStory { get; set; }
        public List<Story> Stories { get; set; }
    }
}