using System.ComponentModel.DataAnnotations;

namespace GuildCars.Models
{
    public class Model
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        public int MakeId { get; set; }
        public Make Make { get; set; }
    }
}
