namespace Hasse.Models
{
    public interface IIdentifiable
    {
        string Id { get; }
    }

    public abstract class Identifiable : IIdentifiable
    {
        public string Id { get; set; }

        public override string ToString()
        {
            return string.Format("{0}#{1}", GetType().Name, Id);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Identifiable;
            return other != null && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return (Id ?? "").GetHashCode();
        }
    }
}