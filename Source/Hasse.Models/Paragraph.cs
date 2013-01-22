namespace Hasse.Models
{
    public abstract class Paragraph
    {
        public string Title { get; set; }
    }

    public class KimalasParagraph : Paragraph
    {
        public string BodyAsDiary { get; set; }
        public string BodyAsHtml { get; set; }
    }
}