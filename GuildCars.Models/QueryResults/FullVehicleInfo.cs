using System.Collections.Generic;
using System.Linq;

namespace GuildCars.Models.QueryResults
{
    public class FullVehicleInfo
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public Model Model { get; set; }
        public bool IsNew { get; set; }
        public int BodyStyleId { get; set; }
        public BodyStyle BodyStyle { get; set; }
        public int Year { get; set; }
        public int TransmissionTypeId { get; set; }
        public TransmissionType TransmissionType { get; set; }
        public string Color { get; set; }
        public string Interior { get; set; }
        public decimal Mileage { get; set; }
        public string VIN { get; set; }
        public decimal MSRP { get; set; }
        public decimal SalePrice { get; set; }
        public string Description { get; set; }
        public short HighwayMpg { get; set; }
        public short CityMpg { get; set; }
        public string Engine { get; set; }
        public IEnumerable<ImagePath> ImagePaths { get; set; }
        public IEnumerable<DetailsInfo> DetailsInfos { get; set; }
        public string OwnerId { get; set; }

        public Vehicle ToVehicle()
        {
            var details = new HashSet<Detail>();
            foreach (var detailInfo in DetailsInfos)
            {
                details.Add(detailInfo.ToDetail());
            }

            return new Vehicle
            {
                Id = Id,
                ModelId = ModelId,
                Model = Model,
                IsNew = IsNew,
                BodyStyleId = BodyStyleId,
                BodyStyle = BodyStyle,
                Year = Year,
                TransmissionTypeId = TransmissionTypeId,
                TransmissionType = TransmissionType,
                Color = Color,
                Interior = Interior,
                Mileage = Mileage,
                VIN = VIN,
                MSRP = MSRP,
                SalePrice = SalePrice,
                Description = Description,
                HighwayMpg = HighwayMpg,
                CityMpg = CityMpg,
                Engine = Engine,
                OwnerId = OwnerId,
                ImagePaths = (ICollection<ImagePath>) ImagePaths,
                Details = details
            };
        }
    }
}
