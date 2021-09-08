using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GuildCars.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public Model Model { get; set; }
        public bool IsNew { get; set; }
        public int BodyStyleId { get; set; }
        public BodyStyle BodyStyle { get; set; }
        
        [Required]
        public int Year { get; set; }
        public int TransmissionTypeId { get; set; }
        public TransmissionType TransmissionType { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Color { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Interior { get; set; }
        
        [Required]
        [Range(0, 999999.99)]
        public decimal Mileage { get; set; }
        
        [Required]
        [MaxLength(17)]
        public string VIN { get; set; }
        
        [Required]
        [Range(0, 999999999.99)]
        public decimal MSRP { get; set; }
        
        [Required]
        [Range(0, 999999999.99)]
        public decimal SalePrice { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        [Range(0, short.MaxValue)]
        public short HighwayMpg { get; set; }
        
        [Required]
        [Range(0, short.MaxValue)]
        public short CityMpg { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Engine { get; set; }
        public ICollection<ImagePath> ImagePaths { get; set; }
        public ICollection<Detail> Details { get; set; }
        public string OwnerId { get; set; }
        public void SetDetails(ICollection<Detail> details)
        {
            Details = details;
        }
    }
}
