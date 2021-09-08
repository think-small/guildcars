using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Models.QueryParams
{
    public class VehicleFilter
    {        
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int[] BodyStyles { get; set; }
        public int[] Makes { get; set; }
        public int[] Models { get; set; }
        public int[] Transmissions { get; set; }
        public string[] VehicleConditions { get; set; }
        public bool IsSearchingForAvailableCars { get; set; }
    }
}
