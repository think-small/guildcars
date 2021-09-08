using GuildCars.Models;
using GuildCars.Services.SaleProcessorService;
using System.IO;
using System.Threading.Tasks;

namespace GuildCars.Services.ReceiptGeneratorService
{
    internal interface IReceiptGeneratorService
    {
        void CreateHeader();
        void AddAmortizedSchedule(AmortizedLoanSchedule loanSchedule);
        string AddApprovalLetter();
        void AddPurchaseInformation();
        void CreateFooter();
        Stream GetPdf();
        void Configure(IVehicleSale purchaseInfo);
    }
}
