using GuildCars.Models.SaleProcessServiceModels;

namespace GuildCars.Models
{
    public class SaleProcessServiceArgs
    {
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public decimal PurchasePrice { get; set; }
        public int TradeInId { get; set; }
        public Vehicle TradeIn { get; set; }
        public int PurchaseTypeId { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
        public int? LoanLength { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? DownPayment { get; set; }
        public decimal? ApprovalAmount { get; set; }
        public FileUploadArgs ApprovalLetter { get; set; }
    }
}
