using System.Collections.Generic;

namespace GuildCars.Models
{
    public class Detail
    {
        public int Id { get; set; }
        public DetailType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsKeyFeature { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
