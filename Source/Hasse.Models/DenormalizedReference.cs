namespace Hasse.Models
{
    public class DenormalizedReference<T> where T : Identifiable
    {
        public string Id { get; set; }

        public static implicit operator DenormalizedReference<T>(T doc)
        {
            return new DenormalizedReference<T> {
                                                    Id = doc.Id,
                                                };
        }
    }
}