using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GuildCars.Models
{
    public class SaleRecord
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string CustomerId { get; set; }
        [MaxLength(50)]
        public string EmployeeId { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int? TradeInId { get; set; }
        public Vehicle TradeIn { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal ExpectedSalePrice { get; set; }
        public int PurchaseTypeId { get; set; }
        public PurchaseType PurchaseType{ get; set; }
        [Column(TypeName="date")]
        public DateTime Date { get; set; }
    }
}
