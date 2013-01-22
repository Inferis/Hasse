using System;
using System.Collections.Generic;

namespace Hasse.Models
{
    public class Story : Identifiable
    {
        public Story()
        {
            CreatedDate = DateTimeOffset.Now;
            Paragraphs = new List<Paragraph>();
            Kids = new List<string>();
            Comments = new List<Comment>();
        }

        public string Title { get; set; }
        public string Slug { get; set; }
        public DateTime PostDate { get; set; }
        public bool Visible { get; set; }

        public DenormalizedReference<User> Author { get; set; }
        public DateTimeOffset CreatedDate { get; private set; }
        public DateTimeOffset UpdatedDate { get; set; }
        
        public StoryProtection StoryProtection { get; set; }
        public List<ExternalStoryReference> ExternalReferences { get; set; } 

        public List<Paragraph> Paragraphs { get; private set; }

        public List<Comment> Comments { get; set; }
        public List<string> Kids { get; set; }
    }

    public class ExternalStoryReference
    {
        public string Type { get; set; }
        public string Reference { get; set; }
    }
}
