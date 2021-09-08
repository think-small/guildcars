using GuildCars.Models;
using System.IO;
using System.Threading.Tasks;

namespace GuildCars.Services.SaleProcessorService
{
    public interface IVehicleSale
    {
        Vehicle Vehicle { get; set; }
        decimal PurchasePrice { get; set; }
        Vehicle TradeIn { get; set; }
        decimal DownPayment { get; set; }
        string CustomerId { get; set; }
        void ProcessTransaction();
        Stream GetPurchaseAgreement();
    }
}
