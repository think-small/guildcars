using System;
using System.Collections.Generic;

namespace GuildCars.Models.QueryResults
{
    public class VehicleOptions
    {
        public List<Tuple<int, string>> Transmissions { get; set; }
        public List<Tuple<int, string>> BodyStyles { get; set; }
        public List<Tuple<int, string>> Details { get; set; }
        public List<Tuple<int, string>> Makes { get; set; }
        public List<Tuple<int, string>> Models { get; set; }
        public Dictionary<Make, IList<Model>> MakesAndModels { get; set; }
    }
}
