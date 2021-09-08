using System.ComponentModel.DataAnnotations;

namespace GuildCars.Models
{
    public class Make
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Make;

            if (other is null)
                return false;

            return Id == other.Id && Name == other.Name;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + Name.GetHashCode();
                return hash;
            }
        }
    }
}
