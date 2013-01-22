namespace Hasse.Models
{
    public class Image
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }

    public class FormattedImage : Image
    {
        public Alignment Alignment { get; set; }
    }

    public enum Alignment
    {
        Left,
        Center,
        Right
    }
}