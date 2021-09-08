using GuildCars.Models.QueryResults;
using System.Linq;

namespace GuildCars.UI.Models
{
    public class InventoryInfoViewModel
    {
        public VehicleOptions VehicleOptions { get; set; }
        public int[] BodyStylesSelected { get; set; }
        public int[] TransmissionTypesSelected { get; set; }
        public int[] ModelsSelected { get; set; }
        public int[] MakesSelected { get; set; }
        public string[] VehicleConditionsSelected { get; set; }

        public int MinPrice { get; set; } = 15000;
        public int MaxPrice { get; set; } = 85000;
    }
}